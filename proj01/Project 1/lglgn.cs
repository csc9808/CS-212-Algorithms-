// C# program project 1 for cs 212

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lg
{
    class Program 
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Program to compute lg lg n!");
            while (true)
            {
                Console.Write("\nEnter your input (n): ");
                double n = double.Parse(Console.ReadLine());
                double lgn = (Lg(n));
                double lglgn = (Lg(Lg(n)));
                if (n <=1 )
                // if the input is less or equal to one it creates undefine solution while computing lglgn thus we will only accept inputs greater than 1 
                    Console.Write("The Input is Invalid, please input a value greater than 1");
                else 
                    Console.WriteLine("lg(lg(({0})) = {1}.",n, lglgn);                               
               
            }
        }
        
        static double Lg(double number )
        {
        double result = 0;
        while (number > 1)
        {
            number /=2;
            result ++;
        }
        return result;
        }
    }

}                 
