using System;
using System.IO;

namespace FileOperationsSample
{
	public struct ListMemmory
	{
		public bool modification;
		public DateTime DateModification;
		public int PageNomber { get; set; }
		public static byte[] Array = new byte[512];
		public ListMemmory(int PageNomber)
		{
			this.modification = false;
			this.DateModification = DateTime.Now;
			this.PageNomber = PageNomber;


		}
		public void DisplayInfo()
		{
			Console.WriteLine($"modification: {modification} \n DateModification: {DateModification}\n PageNomber: {PageNomber} \n");
			for (int i = 0; i < 512; i++)
			{
				Console.WriteLine(Array[i]);
			}
		}
	}
	public class VirtualArray
	{
		static string path = @"states.dat";
		public int SizeArray { get; }
		ListMemmory InMemory;
		public void WriteFile()
		{
			using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
			{

				ListMemmory[] Lists = new ListMemmory[20];
				for (int i = 0; i < 20; i++)
				{
					Lists[i].PageNomber = i;
				}
				Lists[1].PageNomber = 1;
				foreach (ListMemmory s in Lists)
				{

					writer.Write(s.modification);
					//writer.Write(s.DateModification);
					writer.Write(s.PageNomber);
					Random rnd1 = new Random();

					for (int i = 0; i < 512; i++)
					{
						ListMemmory.Array[i] = (byte)rnd1.Next(0, 255);

						writer.Write(ListMemmory.Array[i]);
					}

				}

			}

		}



		public void ReadFile()
		{
			BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open));


			while (reader.PeekChar() > -1)
			{
				bool name = reader.ReadBoolean();
				int capital = reader.ReadInt32();
				byte[] Array = reader.ReadBytes(512);

				Console.WriteLine(" модификация {0}  \n  Лист{1}  \n", name, capital);
				for (int i = 0; i < 512; i++)
				{
					Console.WriteLine(Array[i]);

				}
			}

		}


		public VirtualArray(int SizeArray)
		{

			this.SizeArray = SizeArray;
		}

		public void Search(int SearchIndexElement)// метод вычисления адреса элемента массива с заданным индексом
		{
			InMemory.PageNomber = 0;//нужно убрать


			int SearchPageNomber = SearchIndexElement / 512;
			SearchIndexElement = SearchIndexElement - (SearchPageNomber * 512);
			if (InMemory.PageNomber == null)
			{
				//TODO:ЗАгрузить самую старую страницу (У которой самая старая дата модификации-DateModification)
			}

			if (InMemory.PageNomber != SearchPageNomber)
			{
				//TODO: Загрузить сраницу из файла

			}


		}


		public int ReadValue(int SearchIndexElement) //метод чтения значения элемента массива с заданным индексом в указанную переменную
		{
			int SearchPageNomber = SearchIndexElement / 512;
			SearchIndexElement = SearchIndexElement - (SearchPageNomber * 512);
			if (InMemory.PageNomber == null)
			{
				//TODO:ЗАгрузить самую старую страницу (У которой самая старая дата модификации-DateModification)
			}

			if (InMemory.PageNomber != SearchPageNomber)
			{
				//TODO: Загрузить сраницу из файла

			}
			//TODO:Найти значение элемента и записать его в Element

			int Element = 0;
			return Element;
		}

		public string ChangeValue(int SearchIndexElement) //метод записи заданного значения в элемент массива с указанным индексом
		{
			int SearchPageNomber = SearchIndexElement / 512;
			SearchIndexElement = SearchIndexElement - (SearchPageNomber * 512);
			if (InMemory.PageNomber == null)
			{
				//TODO:ЗАгрузить самую старую страницу (У которой самая старая дата модификации-DateModification) 
			}

			if (InMemory.PageNomber != SearchPageNomber)
			{
				//TODO: Загрузить сраницу из файла

			}
			//TODO: Найти значение элемента и заменить на введенной значение


			return "Замена совершина";
		}

	}
	//пример

	//struct State
	//        {
	//            public string name;
	//            public string capital;
	//            public int area;
	//            public double people;

	//            public State(string n, string c, int a, double p)
	//            {
	//                name = n;
	//                capital = c;
	//                people = p;
	//                area = a;
	//            }
	//        }
	class Program
	{

		static void Main(string[] args)
		{
			VirtualArray i = new VirtualArray(3);
			i.WriteFile();
			i.ReadFile();
			Console.ReadKey();

		}
	}


}