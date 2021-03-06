﻿using Day7.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day7
{
    public static class Tools
    {
        private const string BAG_DEFINITION_ENDING = " bags ";
        private const string CONTAIN = "contain";
        private const string MY_BAG_COLOR = "shiny gold";
        private const string NO_OTHER_BAGS = "no other bags";

        public static int GetNumberOfBagsAbleToContainMyBag(string inputFileName)
        {
            var lines = File.ReadAllLines(inputFileName);
            var bags = LoadBagsList(lines);
            var canContainMyBag = GetBagsThatCanContain(MY_BAG_COLOR, ref bags).ToList();

            var current = canContainMyBag;
            do
            {
                var temporary = new List<string>();
                foreach (var element in current)
                {
                    temporary.AddRange(GetBagsThatCanContain(element, ref bags));
                }

                canContainMyBag.AddRange(current);
                current = temporary;
            }
            while (current.Count > 0);

            return canContainMyBag.Distinct().Count();
        }

        public static int GetNumberOfBagsContainableInMyBag(string inputFileName)
        {
            var lines = File.ReadAllLines(inputFileName);
            var bags = LoadBagsList(lines);

            int numberOfBags = GetNumberOfBagsInside(MY_BAG_COLOR, ref bags);

            return numberOfBags;
        }

        private static IEnumerable<string> GetBagsThatCanContain(string color, ref List<Bag> bags)
        {
            var colors = new List<string>();
            foreach (var bag in bags)
            {
                if (bag.CanContain.Any(x => x.Item2.Color == color))
                {
                    colors.Add(bag.Color);
                }
            }

            return colors;
        }

        private static int GetNumberOfBagsInside(string bagColor, ref List<Bag> bags)
        {
            var bag = bags.First(x => x.Color == bagColor);
            if (bag?.CanContain == null) return 0;

            var sum = 0;
            foreach (var (number, tmpBag) in bag.CanContain)
            {
                sum += number;
                if (tmpBag != null)
                {
                    sum += number * GetNumberOfBagsInside(tmpBag.Color, ref bags);
                }
            }

            return sum;
        }

        private static List<Bag> LoadBagsList(IEnumerable<string> lines)
        {
            var bags = new List<Bag>();
            foreach (var line in lines)
            {
                var definition = line.Split(CONTAIN);
                var color = definition[0].Replace(BAG_DEFINITION_ENDING, string.Empty);
                var containList = definition[1].Replace(".", "").Split(',');
                var canContainList = new List<Tuple<int, Bag>>();
                foreach (var element in containList)
                {
                    if (string.IsNullOrWhiteSpace(element))
                    {
                        throw new Exception("Error 0 in Tools.GetNumberOfBagsAbleToContainMyBag");
                    }

                    if (element.Trim() == NO_OTHER_BAGS)
                    {
                        continue;
                    }

                    var tmp = element.Trim().Split(" ");
                    canContainList.Add(new Tuple<int, Bag>(int.Parse(tmp[0]), new Bag($"{tmp[1]} {tmp[2]}", null)));
                }

                bags.Add(new Bag(color, canContainList));
            }

            return bags;
        }
    }
}