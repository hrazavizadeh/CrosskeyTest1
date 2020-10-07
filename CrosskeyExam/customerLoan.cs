using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrosskeyExam
{
    public class customerLoan
    {
        private string customername = "";
        private double totalLoan = 0.00f;
        private double interest = 0.00f;
        private int years = 0;
        private double monthlyPay=0;



        public customerLoan(string Name, double TotalLoan, double Interest, int Years)
        {
            this.customername = Name;
            this.totalLoan = TotalLoan;
            this.interest = Interest;
            this.years = Years;
            //Calculating customer monthly payment using power method of calss
            monthlyPay = (TotalLoan * (Interest /100)* HPow(1 + (Interest/100), Years*12)) / (HPow(1 + (Interest/100), (Years*12)) - 1);
        }

        //Create Power Method with loop
        private double HPow(double number1, int number2)
        {
            double returnPower = number1;
            for (int counter = 1; counter < number2; counter++)
                returnPower *= number1;
            return returnPower;
        }


        public double MonthlyPay
        {
            get
            {
                return monthlyPay;
            }            
        }

        public string Name
        {
            get
            {
                return customername;
            }
        }

        public double TotalLoan
        {
            get
            {
                return totalLoan;
            }
        }

        public double Years
        {
            get
            {
                return years;
            }
        }

        public double Interest
        {
            get
            {
                return interest;
            }
        } 
    }
}
