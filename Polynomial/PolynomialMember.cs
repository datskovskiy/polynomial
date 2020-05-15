using System;

namespace PolynomialObject
{
    public class PolynomialMember : ICloneable
    {
        public double Degree { get; set; }
        public double Coefficient { get; set; }

        public PolynomialMember(double degree, double coefficient)
        {
            //todo
            throw new NotImplementedException();
        }

        public object Clone()
        {
            //todo
            throw new NotImplementedException();
        }
    }
}
