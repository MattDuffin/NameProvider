﻿using static System.Linq.Enumerable;
using static NameProvider.MaleFemaleGendersAndSurname;
using static NameProvider.ProbabilisticNameFormatter;

namespace NameProvider
{
    using System;
    using System.Collections.Generic;
    using NameFormatter = System.Linq.Expressions.Expression<System.Func<string>>;

    internal class Example
    {
        private static void Main(string[] args)
        {
            var nameFormats =
                (new List<ProbabilisticNameFormatter>
                 {
                     Create(() => $"{Male} {Surname}", 50),
                     Create(() => $"{Male} {Male} {Surname}", 20),
                     Create(() => $"{Male} {Male} {Male} {Surname}", 10),
                     Create(() => $"{Male} {Surname}-{Surname}", 10),
                     Create(() => $"{Male} {Male} {Surname}-{Surname}", 6),
                     Create(() => $"{Male} {Male} {Male} {Surname}-{Surname}", 4)
                 }).AsReadOnly();

            var ci = new CompositeNameGenerator<MaleFemaleGendersAndSurname, RandomNameProvider>(nameFormats);
            var p = ci.NextName();
            //var nn = ci.NextName();
            //var uu = (NameFormatter)(new CompositeNameGeneratorExtensions.EnumModifier<MaleFemaleGendersAndSurname>(null, null).Modify(nameFormats[5].NameFormatter));

            var nameList = new List<string>();

            for (var i = 0; i < 300; i++)
            {
                //    var malenamegenerator = $@"{string.Join(" ", np.NamesForNameType(MaleFemaleGendersAndSurname.Male).Take(random.Next(1, 3)))} {
                //    string.Join("-", np.NamesForNameType(MaleFemaleGendersAndSurname.Surname).Take(random.Next(1, 2)))}";

                nameList.Add(ci.NextName());
            }

            nameList.Clear();

            var rnd = new Random();
            var cp =
                new CompositeNameGenerator<MaleFemaleGendersAndSurname, AlphabeticalNameProvider>(
                    new List<ProbabilisticNameFormatter> { CreateEqualChance(() => $"{Female} the {rnd.Next(1, 5).Ordinal()}") }
                        .AsReadOnly());

            var cl =
                new CompositeNameGenerator<MaleFemaleGendersAndSurname, RandomNameProvider>(
                    new List<ProbabilisticNameFormatter>
                    {
                        Create(
                            () =>
                            $"{cp.NextName()}, daughter of {ci.NextName()}, heir of the lineage {Surname}",
                            80),
                        Create(() => $"{cp.NextName()}, a mongrel", 20)
                    }.AsReadOnly());

            for (var i = 0; i < 100; i++)
            {
                //    var malenamegenerator = $@"{string.Join(" ", np.NamesForNameType(MaleFemaleGendersAndSurname.Male).Take(random.Next(1, 3)))} {
                //    string.Join("-", np.NamesForNameType(MaleFemaleGendersAndSurname.Surname).Take(random.Next(1, 2)))}";

                nameList.Add(cl.NextName());
            }

            //var fnp = new FullNameGenerator();

            var a = cl.Names().Where(n => n.Contains(" Matt ") || n.Contains(" Matthew ")).ToList();
            
            //var maleNames = fnp.Names(Male);
            //var femaleNames = fnp.Names(Female);

            //fnp = new FullNameGenerator();
            //var allNames = fnp.AllNames(Male).Take(200000).ToList();
        }
    }

    public static class ExtensionMethods
    {
        public static string Ordinal(this int number)
        {
            const string TH = "th";
            var s = number.ToString();

            // Negative and zero have no ordinal representation
            if (number < 1) return s;

            number %= 100;
            if ((number >= 11) && (number <= 13)) return s + TH;

            switch (number % 10)
            {
                case 1:
                    return s + "st";
                case 2:
                    return s + "nd";
                case 3:
                    return s + "rd";
                default:
                    return s + TH;
            }
        }
    }
}