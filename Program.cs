using System;
using System.IO;

namespace FileOperationsSample
{
    public struct ListMemmory
    {
        public bool Modification;
        public DateTimeKind DateModification;
        public int PageNomber { get; set; }
        public byte[] ArrayElements { get; set; }
    }
    public class VirtualArray
    {
        static string path = @"ArrayVirtual.txt";
        public int SizeArray { get; }
        ListMemmory InMemory;


        public void WriteFile()//запись в файл
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
            {

                ListMemmory[] Lists = new ListMemmory[20];
                for (int i = 0; i < 20; i++)
                {
                    Lists[i].ArrayElements = new byte[512];
                    for (int j = 0; j < Lists[i].ArrayElements.Length; j++)
                    {
                        Random rnd1 = new Random();
                        Random rnd2 = new Random();
                        byte[] RND2 = new byte[1000];
                        rnd1.NextBytes(Lists[i].ArrayElements);
                    }


                }
                for (int i = 0; i < 20; i++)
                {
                    Lists[i].PageNomber = i;

                }
                foreach (ListMemmory s in Lists)
                {


                    //writer.Write(s.DateModification);
                    writer.Write(s.PageNomber);

                    for (int i = 0; i < 512; i++)
                    {

                        writer.Write(s.ArrayElements[i]);
                    }

                }

            }

        }

        public void ReadFile()//чтение из файла и вывод на консоль
        {
            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                while (reader.PeekChar() > -1)
                {

                    int _PageNomber = reader.ReadInt32();
                    //DateTime DateModification = reader.ReadString();
                    byte[] ArrayElements = reader.ReadBytes(512);

                    Console.WriteLine("  Лист{0}  \n", _PageNomber);
                    for (int i = 0; i < 512; i++)
                    {
                        Console.WriteLine(i + " )Значение " + ArrayElements[i]);

                    }
                }
            }

        }


        public VirtualArray(int SizeArray)
        {

            this.SizeArray = SizeArray;
            InMemory.PageNomber = -1;
            InMemory.Modification = false;
            InMemory.ArrayElements = new byte[512];
        }

        public void Search(int SearchIndexElement)// метод вычисления адреса элемента массива с заданным индексом
        {


            int SearchPageNomber = SearchIndexElement / 512;
            SearchIndexElement = SearchIndexElement - (SearchPageNomber * 512);
            if (InMemory.PageNomber == -1)//загрузка самой старой страницы
            {
                //TODO:Зaгрузить самую старую страницу (У которой самая старая дата модификации-DateModification)
                using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
                {
                    while (reader.PeekChar() > -1)
                    {
                        InMemory.PageNomber = reader.ReadInt32();
                        //string DateModification = reader.ReadString();
                        InMemory.ArrayElements = reader.ReadBytes(512);
                        if (InMemory.PageNomber == 0)
                        {


                            break;
                        }
                    }

                }
            }

            if (InMemory.PageNomber != SearchPageNomber)//загрузка нужной страницы
            {
                if (InMemory.Modification == true)//проверка на изменение страницы в памяти
                {
                    Console.WriteLine("Сработал флапг модификации");
                    using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
                    {
                        if (InMemory.PageNomber == 0)
                        {
                            fileStream.Seek(4, SeekOrigin.Begin);
                            for (int i = 0; i < 512; i++)
                            {

                                fileStream.WriteByte(InMemory.ArrayElements[i]);
                            }

                        }
                        else
                        {
                            fileStream.Seek((516 * (InMemory.PageNomber + 1)) - 512, SeekOrigin.Begin);
                            for (int i = 0; i < 512; i++)
                            {

                                fileStream.WriteByte(InMemory.ArrayElements[i]);
                            }
                        }
                    }



                }
                BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open));


                while (reader.PeekChar() > -1)
                {

                    InMemory.PageNomber = reader.ReadInt32();
                    //string DateModification = reader.ReadString();
                    InMemory.ArrayElements = reader.ReadBytes(512);
                    if (InMemory.PageNomber == SearchPageNomber)
                    {

                        break;
                    }


                }
                reader.Close();

            }
            Console.WriteLine("Адрес элемента:" + "Страница: " + SearchPageNomber + "  Индекс в массиве: " + (SearchIndexElement - 1));

        }


        public int ReadValue(int SearchIndexElement) //метод чтения значения элемента массива с заданным индексом в указанную переменную
        {
            int SearchPageNomber = SearchIndexElement / 512;
            SearchIndexElement = SearchIndexElement - (SearchPageNomber * 512);

            if (InMemory.PageNomber == -1)
            {
                //TODO:Загрузить самую старую страницу (У которой самая старая дата модификации-DateModification)

                using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
                {
                    while (reader.PeekChar() > -1)
                    {
                        InMemory.PageNomber = reader.ReadInt32();
                        //string DateModification = reader.ReadString();
                        InMemory.ArrayElements = reader.ReadBytes(512);
                        if (InMemory.PageNomber == 0)
                        {


                            break;
                        }
                    }
                }
            }

            if (InMemory.PageNomber != SearchPageNomber)
            {
                //TODO: Загрузить сраницу из файла				

                if (InMemory.Modification == true)//проверка на изменение страницы в памяти
                {
                    Console.WriteLine("Сработал флапг модификации");
                    using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
                    {
                        if (InMemory.PageNomber == 0)
                        {
                            fileStream.Seek(4, SeekOrigin.Begin);
                            for (int i = 0; i < 512; i++)
                            {

                                fileStream.WriteByte(InMemory.ArrayElements[i]);
                            }

                        }
                        else
                        {
                            fileStream.Seek((516 * (InMemory.PageNomber + 1)) - 512, SeekOrigin.Begin);
                            for (int i = 0; i < 512; i++)
                            {

                                fileStream.WriteByte(InMemory.ArrayElements[i]);
                            }
                        }
                    }



                }
                BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open));


                while (reader.PeekChar() > -1)
                {

                    InMemory.PageNomber = reader.ReadInt32();
                    //string DateModification = reader.ReadString();
                    InMemory.ArrayElements = reader.ReadBytes(512);
                    if (InMemory.PageNomber == SearchPageNomber)
                    {

                        break;
                    }


                }
                reader.Close();



            }

            int ValueElement = InMemory.ArrayElements[SearchIndexElement - 1];
            return ValueElement;
        }

        public string ChangeValue(int SearchIndexElement) //метод записи заданного значения в элемент массива с указанным индексом
        {
            int SearchPageNomber = SearchIndexElement / 512;
            SearchIndexElement = SearchIndexElement - (SearchPageNomber * 512);
            if (InMemory.PageNomber == -1)
            {
                //TODO:ЗАгрузить самую старую страницу (У которой самая старая дата модификации-DateModification) 
                using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
                {
                    while (reader.PeekChar() > -1)
                    {
                        InMemory.PageNomber = reader.ReadInt32();
                        //string DateModification = reader.ReadString();
                        InMemory.ArrayElements = reader.ReadBytes(512);
                        if (InMemory.PageNomber == 0)
                        {


                            break;
                        }
                    }
                }
            }

            if (InMemory.PageNomber != SearchPageNomber)
            {
                //TODO: Загрузить сраницу из файла
                if (InMemory.Modification == true)//проверка на изменение страницы в памяти
                {
                    Console.WriteLine("Сработал флапг модификации");
                    using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
                    {
                        if (InMemory.PageNomber == 0)
                        {
                            fileStream.Seek(4, SeekOrigin.Begin);
                            for (int i = 0; i < 512; i++)
                            {

                                fileStream.WriteByte(InMemory.ArrayElements[i]);
                            }

                        }
                        else
                        {
                            fileStream.Seek((516 * (InMemory.PageNomber + 1)) - 512, SeekOrigin.Begin);
                            for (int i = 0; i < 512; i++)
                            {

                                fileStream.WriteByte(InMemory.ArrayElements[i]);
                            }
                        }
                    }



                }
                BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open));


                while (reader.PeekChar() > -1)
                {

                    InMemory.PageNomber = reader.ReadInt32();
                    //string DateModification = reader.ReadString();
                    InMemory.ArrayElements = reader.ReadBytes(512);
                    if (InMemory.PageNomber == SearchPageNomber)
                    {

                        break;
                    }


                }
                reader.Close();

            }
            //TODO: Найти значение элемента и заменить на введенной значение
            Console.WriteLine("Введите значение которое хотите записать в место этого: " + InMemory.ArrayElements[SearchIndexElement - 1]);
            Console.WriteLine("");
            Console.Write("Введите число в диапозоне от 0 и до 255:");
            InMemory.ArrayElements[SearchIndexElement - 1] = Convert.ToByte(Console.ReadLine());

            return "Замена совершина";
        }

    }

    class Program
    {

        static void Main(string[] args)
        {

            VirtualArray virtualArray = new VirtualArray(10240);
            virtualArray.WriteFile();
            int Ar = 0;
            int Choose = 0;
            while (Ar == 0)
            {


                Console.WriteLine("Выберите действие нажав нужную клавишу");

                Console.WriteLine("1) Вычисления адреса элемента массива");
                Console.WriteLine("2) Чтения значения элемента массива");
                Console.WriteLine("3) Записи заданного значения в элемент массива");
                Console.WriteLine("4) Вывести данные из файла");
                Console.WriteLine("5) Завершить работу");

                Choose = Convert.ToInt32(Console.ReadLine());

                switch (Choose)
                {
                    case 1:
                        Console.WriteLine("Введите индекс в диапозоне от 0 и до 10240: ");

                        virtualArray.Search(Convert.ToInt32(Console.ReadLine()));
                        Console.WriteLine("Нажмите Enter для продолжения работы");

                        Console.ReadLine();
                        break;
                    case 2:
                        Console.WriteLine("Введите индекс в диапозоне от 0 и до 10240: ");
                        int i = Convert.ToInt32(Console.ReadLine());
                        Console.Clear();
                        Console.WriteLine("Значение элемента =" + virtualArray.ReadValue(i));

                        Console.WriteLine("Нажмите Enter для продолжения работы");
                        Console.ReadLine();
                        break;
                    case 3:
                        Console.WriteLine("Введите индекс в диапозоне от 0 и до 10240: ");

                        Console.WriteLine(virtualArray.ChangeValue(Convert.ToInt32(Console.ReadLine())));
                        Console.WriteLine("Нажмите Enter для продолжения работы");
                        Console.ReadLine();
                        break;
                    case 4:

                        virtualArray.ReadFile();
                        Console.WriteLine("Нажмите Enter для продолжения работы");
                        Console.ReadLine();
                        break;

                }
                if (Choose == 5)
                    break;
                Console.Clear();
            }

        }
    }
}