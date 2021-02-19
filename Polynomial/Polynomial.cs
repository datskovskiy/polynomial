using System;
using System.Collections.Generic;
using System.Linq;
using PolynomialObject.Exceptions;

namespace PolynomialObject
{
    public sealed class Polynomial
    {
        private readonly List<PolynomialMember> _members = new List<PolynomialMember>();
        public Polynomial()
        {
        }

        public Polynomial(PolynomialMember member)
        {
            if (member.Coefficient != 0)
                _members.Add(member);
        }

        public Polynomial(IEnumerable<PolynomialMember> members)
        {
            _members.AddRange(members.Where(m => m.Coefficient != 0));
        }

        public Polynomial((double degree, double coefficient) member)
        {
            if (member.coefficient != 0)
                _members.Add(new PolynomialMember(member.degree, member.coefficient));
        }

        public Polynomial(IEnumerable<(double degree, double coefficient)> members)
        {
            _members.AddRange(members.Select(m => new PolynomialMember(m.degree, m.coefficient)).Where(m => m.Coefficient != 0));
        }

        /// <summary>
        /// The amount of not null polynomial members in polynomial 
        /// </summary>
        public int Count => _members.Count(m => m != null);

        /// <summary>
        /// The biggest degree of polynomial member in polynomial
        /// </summary>
        public double Degree
        {
            get
            {
                return _members.Where(m => m != null)
                    .FirstOrDefault(m => m.Degree.Equals(_members.Max(m => m.Degree))).Degree;
            }
        }

        /// <summary>
        /// Adds new unique member to polynomial 
        /// </summary>
        /// <param name="member">The member to be added</param>
        /// <exception cref="PolynomialArgumentException">Throws when member to add with such degree already exist in polynomial</exception>
        /// <exception cref="PolynomialArgumentNullException">Throws when trying to member to add is null</exception>
        public void AddMember(PolynomialMember member)
        {
            if (member is null)
                throw new PolynomialArgumentNullException("Member cant be null.");

            if (_members.Contains(member))
                throw new PolynomialArgumentException("Member cant be added. Member already exist in polynomial.");

            if (member.Coefficient != 0)
                _members.Add(member);
        }

        /// <summary>
        /// Adds new unique member to polynomial from tuple
        /// </summary>
        /// <param name="member">The member to be added</param>
        /// <exception cref="PolynomialArgumentException">Throws when member to add with such degree already exist in polynomial</exception>
        public void AddMember((double degree, double coefficient) member)
        {
            if (ContainsMember(member.degree))
                throw new PolynomialArgumentException("Member cant be added. Degree already exist in polynomial.");

            AddMember(new PolynomialMember(member.degree, member.coefficient));
        }

        /// <summary>
        /// Removes member of specified degree
        /// </summary>
        /// <param name="degree">The degree of member to be deleted</param>
        /// <returns>True if member has been deleted</returns>
        public bool RemoveMember(double degree)
        {
            return _members.Remove(Find(degree));
        }

        /// <summary>
        /// Searches the polynomial for a method of specified degree
        /// </summary>
        /// <param name="degree">Degree of member</param>
        /// <returns>True if polynomial contains member</returns>
        public bool ContainsMember(double degree)
        {
            return _members.Any(m => m.Degree.Equals(degree));
        }

        /// <summary>
        /// Finds member of specified degree
        /// </summary>
        /// <param name="degree">Degree of member</param>
        /// <returns>Returns the found member or null</returns>
        public PolynomialMember Find(double degree)
        {
            return _members.FirstOrDefault(m => m.Degree.Equals(degree));
        }

        /// <summary>
        /// Gets and sets the coefficient of member with provided degree
        /// If there is no null member for searched degree - return 0 for get and add new member for set
        /// </summary>
        /// <param name="degree">The degree of searched member</param>
        /// <returns>Coefficient of found member</returns>
        public double this[double degree]
        {
            get => Find(degree)?.Coefficient ?? 0;
            set
            {
                var member = Find(degree);
                if (value != 0)
                {
                    if (member is null)
                        AddMember(new PolynomialMember(degree, value));
                    else
                        member.Coefficient = value;
                }
                else if (member != null)
                        RemoveMember(degree);
            }
        }

        /// <summary>
        /// Convert polynomial to array of included polynomial members 
        /// </summary>
        /// <returns>Array with not null polynomial members</returns>
        public PolynomialMember[] ToArray()
        {
            return _members.ToArray();
        }

        /// <summary>
        /// Adds two polynomials
        /// </summary>
        /// <param name="a">The first polynomial</param>
        /// <param name="b">The second polynomial</param>
        /// <returns>New polynomial after adding</returns>
        /// <exception cref="PolynomialArgumentNullException">Throws if either of provided polynomials is null</exception>
        public static Polynomial operator +(Polynomial a, Polynomial b)
        {
            if (a is null || b is null)
                throw new PolynomialArgumentNullException("Polynomial cant be null.");

            foreach (var member in b.ToArray())
                a[member.Degree] += member.Coefficient;

            return a;
        }

        /// <summary>
        /// Subtracts two polynomials
        /// </summary>
        /// <param name="a">The first polynomial</param>
        /// <param name="b">The second polynomial</param>
        /// <returns>Returns new polynomial after subtraction</returns>
        /// <exception cref="PolynomialArgumentNullException">Throws if either of provided polynomials is null</exception>
        public static Polynomial operator -(Polynomial a, Polynomial b)
        {
            if (a is null || b is null)
                throw new PolynomialArgumentNullException("Polynomial cant be null.");

            foreach (var member in b.ToArray())
                a[member.Degree] -= member.Coefficient;

            return a;
        }

        /// <summary>
        /// Multiplies two polynomials
        /// </summary>
        /// <param name="a">The first polynomial</param>
        /// <param name="b">The second polynomial</param>
        /// <returns>Returns new polynomial after multiplication</returns>
        /// <exception cref="PolynomialArgumentNullException">Throws if either of provided polynomials is null</exception>
        public static Polynomial operator *(Polynomial a, Polynomial b)
        {
            if (a is null || b is null)
                throw new PolynomialArgumentNullException("Polynomial cant be null.");

            var result = new Polynomial();

            foreach (var memberA in a._members)
            {
                foreach (var memberB in b._members)
                {
                    var newDegree = memberA.Degree + memberB.Degree;
                    var newCoefficient = memberA.Coefficient * memberB.Coefficient;

                    if (result.ContainsMember(newDegree))
                        result[newDegree] += newCoefficient;
                    else
                        result.AddMember(new PolynomialMember(newDegree, newCoefficient));

                }
            }
            
            return result;
        }

        /// <summary>
        /// Adds polynomial to polynomial
        /// </summary>
        /// <param name="polynomial">The polynomial to add</param>
        /// <returns>Returns new polynomial after adding</returns>
        /// <exception cref="PolynomialArgumentNullException">Throws if provided polynomial is null</exception>
        public Polynomial Add(Polynomial polynomial)
        {
            return this + polynomial;
        }

        /// <summary>
        /// Subtracts polynomial from polynomial
        /// </summary>
        /// <param name="polynomial">The polynomial to subtract</param>
        /// <returns>Returns new polynomial after subtraction</returns>
        /// <exception cref="PolynomialArgumentNullException">Throws if provided polynomial is null</exception>
        public Polynomial Subtraction(Polynomial polynomial)
        {
            return this - polynomial;
        }

        /// <summary>
        /// Multiplies polynomial with polynomial
        /// </summary>
        /// <param name="polynomial">The polynomial for multiplication </param>
        /// <returns>Returns new polynomial after multiplication</returns>
        /// <exception cref="PolynomialArgumentNullException">Throws if provided polynomial is null</exception>
        public Polynomial Multiply(Polynomial polynomial)
        {
            return this * polynomial;
        }

        /// <summary>
        /// Adds polynomial and tuple
        /// </summary>
        /// <param name="a">The polynomial</param>
        /// <param name="b">The tuple</param>
        /// <returns>Returns new polynomial after adding</returns>
        public static Polynomial operator +(Polynomial a, (double degree, double coefficient) b)
        {
            return a + new Polynomial(b);
        }

        /// <summary>
        /// Subtract polynomial and tuple
        /// </summary>
        /// <param name="a">The polynomial</param>
        /// <param name="b">The tuple</param>
        /// <returns>Returns new polynomial after subtraction</returns>
        public static Polynomial operator -(Polynomial a, (double degree, double coefficient) b)
        {
            return a - new Polynomial(b);
        }

        /// <summary>
        /// Multiplies polynomial and tuple
        /// </summary>
        /// <param name="a">The polynomial</param>
        /// <param name="b">The tuple</param>
        /// <returns>Returns new polynomial after multiplication</returns>
        public static Polynomial operator *(Polynomial a, (double degree, double coefficient) b)
        {
            return a * new Polynomial(b);
        }

        /// <summary>
        /// Adds tuple to polynomial
        /// </summary>
        /// <param name="member">The tuple to add</param>
        /// <returns>Returns new polynomial after adding</returns>
        public Polynomial Add((double degree, double coefficient) member)
        {
            return this + new Polynomial(member);
        }

        /// <summary>
        /// Subtracts tuple from polynomial
        /// </summary>
        /// <param name="member">The tuple to subtract</param>
        /// <returns>Returns new polynomial after subtraction</returns>
        public Polynomial Subtraction((double degree, double coefficient) member)
        {
            return this - new Polynomial(member);
        }

        /// <summary>
        /// Multiplies tuple with polynomial
        /// </summary>
        /// <param name="member">The tuple for multiplication </param>
        /// <returns>Returns new polynomial after multiplication</returns>
        public Polynomial Multiply((double degree, double coefficient) member)
        {
            return this * new Polynomial(member);
        }
    }
}
