/*Разработать класс «Счет для оплаты». В классе предусмотреть следующие поля:
■ оплата за день;
■ количество дней;
■ штраф за один день задержки оплаты;
■ количество дней задержи оплаты;
■ сумма к оплате без штрафа(вычисляемое поле);
■ штраф(вычисляемое поле);
■ общая сумма к оплате(вычисляемое поле).
В классе объявить статическое свойство типа bool,
значение которого влияет на процесс форматирования
объектов этого класса.Если значение этого свойства равно true, тогда сериализуются и десериализируются все
поля, если false — вычисляемые поля не сериализуются.
Разработать приложение, в котором необходимо продемонстрировать использование этого класса, результаты
должны записываться и считываться из файла.*/
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using static System.Console;

namespace Home_Task_10
{
    [Serializable]
    public class PayBill : ISerializable 
    {
        public double paybyOneday { get; set; } //оплата за день
        public int numberofDays { get; set; } //количество дней
        public double fine { get; set; } //штраф за один день задержки оплаты
        public int numDayFine { get; set; } //количество дней задержи оплаты
        public double sumNoFine;
        public double sumFine;
        public double rezult;
        public static bool paybill;

        public double Sum_no_Fine()
        {
            return sumNoFine = paybyOneday * numberofDays;
        }
        public double Sum_Fine()
        {
            return sumFine = fine * numDayFine;
        }
        public double Rezult()
        {
            return rezult = sumNoFine + sumFine;
        }
        public PayBill() { }
        public void setPayBill()
        {
            WriteLine("Давайте создадим счет для оплаты");
            WriteLine("Введите сумму оплаты за день");
            paybyOneday = Convert.ToDouble(ReadLine());
            WriteLine("Введите количество дней");
            numberofDays = Convert.ToInt32(ReadLine());
            WriteLine("Введите штраф за один день задержки оплаты");
            fine = Convert.ToDouble(ReadLine());
            WriteLine("Введите количество дней задержи оплаты");
            numDayFine = Convert.ToInt32(ReadLine());        
        }

        public override string ToString()
        {
            return $@"
                   Итак имеем:
                      ■ оплата за день - {paybyOneday}
                      ■ количество дней - {numberofDays};
                      ■ штраф за один день задержки оплаты - {fine};
                      ■ количество дней задержи оплаты - {numDayFine};
                   Итого:
                      ■ сумма к оплате без штрафа - {sumNoFine};
                      ■ штраф - {sumFine};
                      ■ общая сумма к оплате - {rezult}";
        }
        private PayBill(SerializationInfo info, StreamingContext context)
        {
            paybyOneday = info.GetDouble("оплата за день");
            numberofDays = info.GetInt32("количество дней");
            fine = info.GetDouble("штраф за один день задержки оплаты");
            numDayFine = info.GetInt32("количество дней задержи оплаты");
            if (paybill)
            {
                sumNoFine = info.GetDouble("сумма к оплате без штрафа");
                sumFine = info.GetDouble("штраф");
                rezult = info.GetDouble("общая сумма к оплате");
            }
        }
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("оплата за день", paybyOneday);
            info.AddValue("количество дней", numberofDays);
            info.AddValue("штраф за один день задержки оплаты", fine);
            info.AddValue("количество дней задержи оплаты", numDayFine);
            if (paybill)
            {
                info.AddValue("сумма к оплате без штрафа", sumNoFine);
                info.AddValue("штраф", sumFine);
                info.AddValue("общая сумма к оплате", rezult);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            ForegroundColor = ConsoleColor.Yellow;
            PayBill pb = new PayBill();
            pb.setPayBill();
            pb.Sum_no_Fine();
            pb.Sum_Fine();
            pb.Rezult();
            WriteLine(pb);
            WriteLine("Была ли произведена оплата? Нажмите: 1 - ДА, 2 - НЕТ.");
            int answer = Convert.ToInt32(ReadLine());
            if (answer == 1)
            {
                PayBill.paybill = true;
                WriteLine("Полная сериализация!");
                SoapFormatter soapFormat = new SoapFormatter();
                try
                {
                    using (Stream fStream = File.Create("test1.soap"))
                    {
                        soapFormat.Serialize(fStream, pb);
                    }
                    WriteLine("SoapSerialize OK!\n");

                    PayBill p = null;
                    using (Stream fStream = File.OpenRead("test1.soap"))
                    {
                        p = (PayBill)soapFormat.Deserialize(fStream);
                    }
                    WriteLine(p);
                }
                catch (Exception ex)
                {
                    WriteLine(ex);
                }
            }
            else
            {
                PayBill.paybill = false;
                WriteLine("Частичная сериализация!");
                SoapFormatter soapFormat = new SoapFormatter();
                try
                {
                    using (Stream fStream = File.Create("test2.soap"))
                    {
                        soapFormat.Serialize(fStream, pb);
                    }
                    WriteLine("SoapSerialize OK!\n");

                    PayBill p = null;
                    using (Stream fStream = File.OpenRead("test2.soap"))
                    {
                        p = (PayBill)soapFormat.Deserialize(fStream);
                    }
                    WriteLine(p);
                }
                catch (Exception ex)
                {
                    WriteLine(ex);
                }
            }
            ReadKey();

        }
    }
}
