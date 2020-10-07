using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrosskeyExam
{
    public class customerLoan
    {
        private string name = "";
        private double totalLoan = 0.00f;
        private double interest = 0.00f;
        private int years = 0;
        private double monthlyPay=0;


        public customerLoan(string Name, double TotalLoan, double Interest, int Years)
        {
            this.name = Name;
            this.totalLoan = TotalLoan;
            this.interest = Interest;
            this.years = Years;
            monthlyPay = (TotalLoan * (Interest /100)* HPow(1 + (Interest/100), Years*12)) / (HPow(1 + (Interest/100), (Years*12)) - 1);
        }

        private double HPow(double p1,int p2)
        {
            double rp = p1;
            for (int i = 1; i < p2; i++)
                rp *= p1;
            return rp;
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
                return name;
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
