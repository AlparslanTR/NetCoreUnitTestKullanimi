using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestApp
{
    public class Calculator
    {
        private ICalculatorService _calculatorService { get; set; } 
        public Calculator(ICalculatorService calculatorService) 
        {
            _calculatorService= calculatorService;
        }

        public int Add(int a, int b)
        {
            return _calculatorService.add(a, b);
        }
    }
}
