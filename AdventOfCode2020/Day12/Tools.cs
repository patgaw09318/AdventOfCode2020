﻿using System;
using System.IO;

namespace Day12
{
    public static class Tools
    {
        private const char EAST = 'E';
        private const char FORWARD = 'F';
        private const char LEFT = 'L';
        private const char NORTH = 'N';
        private const char RIGHT = 'R';
        private const char SOUTH = 'S';
        private const char WEST = 'W';

        public static long GetResultPart1(string inputFileName)
        {
            var lines = File.ReadAllLines(inputFileName);
            var northSouthValue = 0;
            var eastWestValue = 0;
            var currentDirection = EAST;
            foreach (var line in lines)
            {
                ExecuteCommand(line, ref northSouthValue, ref eastWestValue, ref currentDirection);
            }

            return Math.Abs(northSouthValue) + Math.Abs(eastWestValue);
        }

        private static void ExecuteCommand(string command, ref int northSouthValue, ref int eastWestValue, ref char currentDirection)
        {
            var type = command[0];
            var value = int.Parse(command.Substring(1));
            if (type == FORWARD)
            {
                type = currentDirection;
            }
            else if (type == RIGHT || type == LEFT)
            {
                var turnTimes = value / 90;
                currentDirection = GetCalculateDirection(currentDirection, type, turnTimes);
            }

            switch (type)
            {
                case NORTH:
                    northSouthValue += value;
                    break;
                case SOUTH:
                    northSouthValue -= value;
                    break;
                case EAST:
                    eastWestValue += value;
                    break;
                case WEST:
                    eastWestValue -= value;
                    break;
            }
        }

        private static char GetCalculateDirection(char direction, char turn, int times)
        {
            times %= 4;
            if (times == 4)
            {
                return direction;
            }

            for (int i = 0; i < times; i++)
            {
                direction = NextDirection(direction, turn);
            }

            return direction;
        }

        private static char NextDirection(char direction, char turn)
        {
            return direction switch
            {
                NORTH => turn == RIGHT ? EAST : WEST,
                SOUTH => turn == RIGHT ? WEST : EAST,
                EAST => turn == RIGHT ? SOUTH : NORTH,
                WEST => turn == RIGHT ? NORTH : SOUTH,
                _ => throw new Exception("Error in Tools.NextDirection")
            };
        }
    }
}