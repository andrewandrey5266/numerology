using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class DocGenerator
{
    private static string Path =>
      Application.persistentDataPath;
    //Application.dataPath;
    
    public static void GenerateConsultationDoc()
    {
        StringBuilder content = new StringBuilder();

        int maxValue = CalculatePortrait.positions.Skip(1).Take(6).Max();
        int soulLevelIndex = maxValue < 1
            ? 3
            : maxValue < 8
                ? 0
                : maxValue < 15
                    ? 1
                    : maxValue < 22
                        ? 2
                        : 3;

        content.Append(
            //energy
            $"\nКоличество мужской энергии - {CalculatePortrait.malePoints}, женской энергии - {CalculatePortrait.femalePoints},"
            //max val
            + $"\n\n{maxValue} - {PositionMeaning.SoulLevel[soulLevelIndex]}");

        List<int> usedNumbers = new List<int>();

        for (int i = 1; i <= 5; i++)
        {
            string energyDescription;

            if (i == 4)
            {
                int length = PositionMeaning.EnergyDescriptions[CalculatePortrait.positions[i]].Length;
                int startIndex = PositionMeaning.EnergyDescriptions[CalculatePortrait.positions[i]]
                    .IndexOf("\n\n\"-\"", StringComparison.InvariantCulture);

                energyDescription = PositionMeaning.EnergyDescriptions[CalculatePortrait.positions[i]]
                    .Substring(startIndex + 2, length - startIndex - 2);
            }
            else
            {
                energyDescription = PositionMeaning.EnergyDescriptions[CalculatePortrait.positions[i]];
            }

            content.Append(
                // positions
                "\n\n----------------"
                + "\n" + PositionMeaning.PositionHeaders[i]
                + "\n----------------"
                + "\n" + PositionMeaning.PositionsDescription[i]
                + "\n\n" + PositionMeaning.EnergyNames[CalculatePortrait.positions[i]].ToUpper()
                + "\n" + energyDescription);

            if (i == 2)
            {
                usedNumbers.Add(CalculatePortrait.positions[i] - 1);
                if (CalculatePortrait.positions[i] - 1 > 0 && CalculatePortrait.positions[i] - 1 <= 12)
                {
                    content.Append(
                        "\n\n----------------"
                        + "\n" + PositionMeaning.SecondPositionLessonHeader1
                        + "\n----------------"
                        + "\n" + PositionMeaning.SecondPositionLessonHeader2
                        + "\n\n" + PositionMeaning.SecondPositionLessonMeaning[CalculatePortrait.positions[i] - 1]);
                }

                for (int j = 1; j <= 6; j++)
                {
                    if (j == 2)
                    {
                        continue;
                    }

                    if (!CalculatePortrait.sign[j] && CalculatePortrait.positions[j] - 1 > 0 &&
                        CalculatePortrait.positions[j] - 1 <= 12
                        && !usedNumbers.Contains(CalculatePortrait.positions[j] - 1))
                    {
                        usedNumbers.Add(CalculatePortrait.positions[j] - 1);
                        content.Append(
                            $"\n\n" + PositionMeaning.SecondPositionLessonMeaning[
                                CalculatePortrait.positions[j] - 1]);
                    }
                }
            }
        }

        content.Append(
            // positions
            "\n\n----------------"
            + "\n" + PositionMeaning.PositionHeaders[6]
            + "\n----------------"
            + "\n" + PositionMeaning.PositionsDescription[6]
            + $"\n\n{CalculatePortrait.positions[6].ToRoman()} {PositionMeaning.EnergyNames[CalculatePortrait.positions[6]]}"
            + "\n" + PositionMeaning.SixPositionMeanings[CalculatePortrait.positions[6]]);

        content.Append(
            // positions
            "\n\n----------------"
            + "\n" + PositionMeaning.PositionHeaders[7]
            + "\n----------------"
            + "\n" + PositionMeaning.PositionsDescription[7]
            + $"\n\n{CalculatePortrait.positions[7].ToRoman()} {PositionMeaning.EnergyNames[CalculatePortrait.positions[7]]}"
            + "\n" + PositionMeaning.SevenPositionMeanings[CalculatePortrait.positions[7]]);

        content.Append(
            // positions
            "\n\n----------------"
            + "\n" + PositionMeaning.PositionHeaders[8]
            + "\n----------------"
            + "\n" + PositionMeaning.PositionsDescription[8]
            + "\n\n" + PositionMeaning.EightPositionMeaning[CalculatePortrait.positions[8]]);

        content.Append(
            // positions
            "\n\n----------------"
            + "\n" + PositionMeaning.PositionHeaders[12]
            + "\n----------------"
            + "\n" + PositionMeaning.PositionsDescription[12]
            + "\n\n" + PositionMeaning.TwelvePositionMeaning[CalculatePortrait.positions[12]]);

        content.Append("\n\n\n----------------" +
                       "\nКармический портрет\nКем был(а) и что делал(а) в прошлой жизни?" +
                       "\n----------------");
        usedNumbers = new List<int> { CalculatePortrait.positions[9] };

        content.Append(
            "\n\n9п: "
            + $"\n\n{CalculatePortrait.positions[9].ToRoman()} - " +
            PositionMeaning.NinethPosition[CalculatePortrait.positions[9]]);
        for (int i = 1; i <= 6; i++)
        {
            if ((!CalculatePortrait.sign[i] || i == 4) && CalculatePortrait.sexpoints[i] == 2
                                                       && !usedNumbers.Contains(CalculatePortrait.positions[i]))
            {
                usedNumbers.Add(CalculatePortrait.positions[i]);
                content.Append($"\n\n{CalculatePortrait.positions[i].ToRoman()} - " +
                               PositionMeaning.NinethPosition[CalculatePortrait.positions[i]]);
            }
        }

        usedNumbers = new List<int> { CalculatePortrait.positions[10] };
        content.Append(
            "\n\n10п: "
            + $"\n\n{CalculatePortrait.positions[10].ToRoman()} - " +
            PositionMeaning.TenthPosition[CalculatePortrait.positions[10]]);

        for (int i = 1; i <= 6; i++)
        {
            if ((!CalculatePortrait.sign[i] || i == 4) && CalculatePortrait.sexpoints[i] < 2
                                                       && !usedNumbers.Contains(CalculatePortrait.positions[i]))
            {
                usedNumbers.Add(CalculatePortrait.positions[i]);
                content.Append($"\n\n{CalculatePortrait.positions[i].ToRoman()} - " +
                               PositionMeaning.TenthPosition[CalculatePortrait.positions[i]]);
            }
        }

        content.Append("\n\n\nКармические задачи");

        for (int i = 1; i <= 2; i++)
        {
            content.Append(
                $"\n\n{CalculatePortrait.kzs[i].ToRoman().ZeroTo22()} - " +
                PositionMeaning.KarmaMeanings[CalculatePortrait.kzs[i]]);
        }

        content.Append(
            // positions
            "\n\n----------------"
            + "\nПроработка"
            + "\n----------------"
            + "\n\n" + PositionMeaning.ProrabotkaDescription);

        for (int i = 1; i <= 3; i++)
        {
            int indexOfMinus = i == 1
                ? PositionMeaning.EnergyDescriptions[CalculatePortrait.prs[i]].Length
                : PositionMeaning.EnergyDescriptions[CalculatePortrait.prs[i]]
                    .IndexOf("\n\n\"-\"", StringComparison.InvariantCulture);

            content.Append(
                "\n\n" + PositionMeaning.ProrabotkaHeaders[i]
                       + "\n\n" + PositionMeaning.EnergyNames[CalculatePortrait.prs[i]].ToUpper()
                       + "\n" + PositionMeaning.EnergyDescriptions[CalculatePortrait.prs[i]]
                           .Substring(0, indexOfMinus));
        }

        content.Append(
            // positions
            "\n\n----------------"
            + "\nТеневой портрет"
            + "\n----------------"
            + "\n\n" + PositionMeaning.ShadowPortraitDesc);

        foreach (var c in PositionMeaning.ShadowPortraitPosDesc)
        {
            int searchValue = CalculatePortrait.shp[c.Key];
            if (searchValue == 22)
            {
                searchValue = 0;
            }

            string arcaneDesc = PositionMeaning.ShadowPortraits[searchValue];

            int keyIndex = arcaneDesc.IndexOf($"{c.Key} —");
            string cut1 = arcaneDesc.Substring(keyIndex, arcaneDesc.Length - keyIndex);

            int newlineIndex = cut1.IndexOf("\n");
            if (newlineIndex == -1)
            {
                newlineIndex = cut1.Length;
            }

            int beginningIndex = $"{c.Key} — ".Length;
            string cut2 = cut1.Substring(beginningIndex, newlineIndex - beginningIndex);

            string cut3 = CalculatePortrait.shp[c.Key].ToRoman() + ". " + cut2;

            content.Append($"\n\n\n{c.Key} — {c.Value}"
                           + "\n\n" + cut3);
        }

        string fileName = $"{CalculatePortrait.day}.{CalculatePortrait.month}.{CalculatePortrait.year}";
        
        File.WriteAllText(Path + $"/{fileName}.txt",
            content.ToString());
    }

    public static void GenerateRelationshipDoc()
    {
        StringBuilder content = new StringBuilder();
        string[] pos = new string[13];
        string[] prs = new string[4];

        for (int i = 1; i <= 12; i++)
        {
            if (i >= 9 && i <= 11)
            {
                continue;
            }

            StringBuilder c = new StringBuilder();
            c.Append(
                $"{PositionMeaning.RelPosHeaders[i]}\n-----------------"
                + $"\n{CalculatePortrait.positions[i].ToRoman()}." +
                $" {PositionMeaning.EnergyNames[CalculatePortrait.positions[i]]}\n");

            if (i >= 2 && i <= 6)
            {
                string desc = !CalculatePortrait.sign[i] || i == 3
                    ? PositionMeaning.RelMinusMeaning[CalculatePortrait.positions[i]]
                    : PositionMeaning.RelPlusMeaning[CalculatePortrait.positions[i]];
                    
                c.Append($"{desc}\n");
            }
            
            pos[i] = $"{c}\n";
        }

        for (int i = 1; i <= 3; i++)
        {
            var desc = i == 3 ? $"{PositionMeaning.RelMinusMeaning[CalculatePortrait.prs[i]]}\n" : string.Empty;

            prs[i] =
                $"{PositionMeaning.RelPrHeaders[i]}\n-----------------"
                + $"\n{CalculatePortrait.prs[i].ToRoman()}." +
                $" {PositionMeaning.EnergyNames[CalculatePortrait.prs[i]]}\n{desc}\n";

        }

        content.Append(pos[1]);
        content.Append(prs[1]);
        
        content.Append(pos[2]);
        content.Append(prs[2]);
        
        content.Append(pos[4]);
        content.Append(pos[5]);
        content.Append(pos[6]);

        content.Append(pos[3]);
        content.Append(prs[3]);

        content.Append(pos[7]);
        content.Append(pos[8]);
        content.Append(pos[12]);
        
        string fileName = $"{CalculatePortrait.day}.{CalculatePortrait.month}.{CalculatePortrait.year}"
                          + $" {CalculatePortrait.day2}.{CalculatePortrait.month2}.{CalculatePortrait.year2}";
        
        File.WriteAllText(Path + $"/{fileName}.txt",
            content.ToString());
    }
}
