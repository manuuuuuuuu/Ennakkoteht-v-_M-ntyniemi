using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mäntyniemi_Ennakkotehtävä
{
    class Ennakkotehtava
    {
        static void Main()
        {
            while (true)
            {
                BusinnesIdSpecification BusinnesId = new BusinnesIdSpecification();
                Console.WriteLine("Input Businnes ID");

                BusinnesId.id = Console.ReadLine();
                bool isSatisfied = BusinnesId.IsSatisfiedBy(BusinnesId);

                if (isSatisfied) Console.WriteLine("Valid Businnes ID");
            }
        }
    }
    class BusinnesIdSpecification : ISpecification<BusinnesIdSpecification>
    {
        public string id;

        ISpecification<string> IdSpecification = new AndSpecification<string>(new IdLengthSpecification(), new IdCharacterSpecification());
        public IEnumerable<string> ReasonsForDissatisfaction => throw new NotImplementedException();

        public bool IsSatisfiedBy(BusinnesIdSpecification entity)
        {
            return IdSpecification.IsSatisfiedBy(entity.id);
        }
    }

    internal class AndSpecification<TEntity> : ISpecification<TEntity>
    {
        private ISpecification<TEntity> _specification1;
        private ISpecification<TEntity> _specification2;


        public IEnumerable<string> ReasonsForDissatisfaction => throw new NotImplementedException();

        internal AndSpecification(ISpecification<TEntity> specification1, ISpecification<TEntity> specification2)
        {
            this._specification1 = specification1;
            this._specification2 = specification2;
        }


        public bool IsSatisfiedBy(TEntity entity)
        {
            return (this._specification1.IsSatisfiedBy(entity) && this._specification2.IsSatisfiedBy(entity));
        }
    }

    public interface ISpecification<in TEntity>
    {
        IEnumerable<string> ReasonsForDissatisfaction { get; }
        bool IsSatisfiedBy(TEntity entity);
    }

    public class IdLengthSpecification : ISpecification<string>
    {
        public IEnumerable<string> ReasonsForDissatisfaction => throw new NotImplementedException();

        public bool IsSatisfiedBy(string id)
        {
            if (id.Length > 9) { Console.WriteLine("Too many characters"); return false; }

            if (id.Length < 9) { Console.WriteLine("Too few characters"); return false; }

            return true;
        }
    }

    public class IdCharacterSpecification : ISpecification<string>
    {
        public IEnumerable<string> ReasonsForDissatisfaction => throw new NotImplementedException();

        public bool IsSatisfiedBy(string id)
        {
            if (id.Any(char.IsLetter)) { Console.WriteLine("Businnes ID can't contain letters"); return false; }

            for (int i = 0; i < id.Length; i++)
            {
                if (char.IsSymbol(id[i]) && i != 7)
                {
                    Console.WriteLine("Businnes ID can't contain symbols");
                    return false;
                }
                if (id[i] != '-' && i == 7)
                {
                    Console.WriteLine("Businnes ID must contain a dash");
                    return false;
                }
            }

            return true;
        }
    }

    public static class SpecificationExtensionMethods
    {
        public static ISpecification<TEntity> And<TEntity>(
          this ISpecification<TEntity> specification1,
          ISpecification<TEntity> specification2)
        {
            return new AndSpecification<TEntity>(
              specification1, specification2);
        }
    }
}
