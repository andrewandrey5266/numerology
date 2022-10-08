using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CalculatePortrait : MonoBehaviour
{
	private static TMP_InputField _day, _month, _year, _day2, _month2, _year2;

	public static int[] positions = new int[13];
	public static int[] prs = new int[4];//0 index is redundant 
	public static bool[] sign = new bool[7];//0 index is redundant 
	public static int[] sexpoints = new int[7];//0 index is redundant 
	public static int[] kzs = new int[3];
	public static Dictionary<string, int> shp = new Dictionary<string, int>();// a b c | d e f | h g1 g2 | i

	public static int malePoints;
	public static int femalePoints;
	public static int day;
	public static int month;
	public static int year;
	
	public static int day2;
	public static int month2;
	public static int year2;
	
	public void Start()
	{
		_day = GameObject.Find("day").GetComponent<TMP_InputField>();
		_month = GameObject.Find("month").GetComponent<TMP_InputField>();
		_year = GameObject.Find("year").GetComponent<TMP_InputField>();
		
		_day2 = GameObject.Find("day2").GetComponent<TMP_InputField>();
		_month2 = GameObject.Find("month2").GetComponent<TMP_InputField>();
		_year2 = GameObject.Find("year2").GetComponent<TMP_InputField>();
	}

	public static void ActivateScreenManually()
	{
		_day.text = _month.text = _year.text = _day2.text = _month2.text = _year2.text = "";
		
		_day.onSubmit.AddListener(x => _month.ActivateInputField());
		_month.onSubmit.AddListener(x => _year.ActivateInputField());

		var cbt = GameObject.Find("calculatePortraitButton").transform;
		if (UIHandler.MultipleDates)
		{
			cbt.localPosition = new Vector3(cbt.localPosition.x, 169.1f, cbt.localPosition.z);
			_day2.gameObject.SetActive(true);
			_month2.gameObject.SetActive(true);
			_year2.gameObject.SetActive(true);
			
			_year.onSubmit.AddListener(x => _day2.ActivateInputField());
			_day2.onSubmit.AddListener(x => _month2.ActivateInputField());
			_month2.onSubmit.AddListener(x => _year2.ActivateInputField());
		}
		else
		{
			cbt.localPosition = new Vector3(cbt.localPosition.x, 192.9f, cbt.localPosition.z);
			_day2.gameObject.SetActive(false);
			_month2.gameObject.SetActive(false);
			_year2.gameObject.SetActive(false);
		}

		ClearPot();
	}

	public void Calculate()
	{
		day = int.Parse(_day.text);
		month = int.Parse(_month.text);
		year = int.Parse(_year.text);

		if (UIHandler.MultipleDates)
		{
			day2 = int.Parse(_day2.text);
			month2 = int.Parse(_month2.text);
			year2 = int.Parse(_year2.text);
		}
		
		positions[1] = (day + day2).ApplyReduction();
		positions[2] = (month + month2).ApplyReduction();
		positions[3] = (year.ToString().Sum(c => c - '0')
		                + year2.ToString().Sum(c => c - '0')).ApplyReduction();
	
		positions[4] = (positions[1] + positions[2]).ApplyReduction();
		positions[5] = (positions[2] + positions[3]).ApplyReduction();
		positions[6] = (positions[4] + positions[5]).ApplyReduction();
		positions[7] = (positions[1] + positions[5]).ApplyReduction();
		positions[8] = (positions[2] + positions[6]).ApplyReduction();
		positions[9] = Math.Abs(positions[1] - positions[2]);
		positions[10] = Math.Abs(positions[2] - positions[3]);
		positions[11] = Math.Abs(positions[9] - positions[10]);
		positions[12] = (positions[7] + positions[8]).ApplyReduction();
		positions[0] = positions[2];

		kzs[1] = positions[11];
		kzs[2] = (positions[9] + positions[10] + positions[11] ).ApplyReduction();
		prs[1] = (positions[1] + positions[4] + positions[5]).ApplyReduction();
		prs[2] = (positions[2] + positions[4] + positions[5] + positions[6]).ApplyReduction();
		prs[3] = (positions[3] + positions[5] + positions[6]).ApplyReduction();

		shp["A"] = (positions[1] + positions[4]).ApplyReduction();//a
		shp["B"] = (positions[4] + positions[2]).ApplyReduction();//b
		shp["C"] = (positions[2] + positions[5]).ApplyReduction();//c
		shp["D"] = (positions[5] + positions[3]).ApplyReduction();//d
		shp["E"] = (positions[4] + positions[6]).ApplyReduction();//e
		shp["F"] = (positions[6] + positions[5]).ApplyReduction();//f
		
		shp["H"] =  (shp["A"] + shp["E"]).ApplyReduction();//h
		shp["G1"] = (shp["C"] + shp["D"]).ApplyReduction();//g1
		shp["G2"] = (shp["B"] + shp["F"]).ApplyReduction();//g2
		shp["I"] =  (shp["G1"] + shp["G2"]).ApplyReduction();//i

		malePoints = femalePoints = 0;
		for (int i = 1; i <= 6; i++)
		{
			var sexPointStr = PositionMeaning.SexPoints[positions[i]];
			int point = sexPointStr.Length > 1 ? sexPointStr[1] - '0' : 0;
			sexpoints[i] = point;
			if (sexPointStr[0] == 'ж')
			{
				femalePoints += point;
			}else if (sexPointStr[0] == 'м')
			{
				malePoints += point;
			}
			GameObject.Find("e" + i).GetComponent<TMP_Text>().text = sexPointStr;
		}
		
		GameObject.Find("2k").GetComponent<TMP_Text>().text = positions[0].ToRoman().ZeroTo22();

		var sameValues = positions.Skip(1).Take(6).GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
		
		for (int i = 1; i <= 12; i++)
		{
			string suffix = i >= 1 && i <= 6 && i != 4 ? sameValues.Contains(positions[i]) ? "-" : "+" : i == 4 ? "-" : i == 8 ? "+" : string.Empty;
			if (i < 7)
			{
				sign[i] = suffix == "+";
			}
			
			GameObject.Find("p" + i).GetComponent<TMP_Text>().text = positions[i].ToRoman().ZeroTo22() + suffix;
		}

		GameObject.Find("kz1").GetComponent<TMP_Text>().text = kzs[1].ToRoman().ZeroTo22();
		GameObject.Find("kz2").GetComponent<TMP_Text>().text = kzs[2].ToRoman().ZeroTo22();

		for (int i = 1; i <= 3; i++)
		{
			GameObject.Find("pr" + i).GetComponent<TMP_Text>().text = prs[i].ToRoman().ZeroTo22();
		}

		if (!UIHandler.MultipleDates)
		{
			GameObject[] shpGameObjects = { GameObject.Find("shp1"), GameObject.Find("shp2") };
			int index = 0;
			shpGameObjects[0].GetComponent<TMP_Text>().text = "";
			shpGameObjects[1].GetComponent<TMP_Text>().text = "";
			foreach (var c in shp)
			{
				shpGameObjects[index / 5].GetComponent<TMP_Text>().text += $"{c.Key} — {c.Value.ToRoman()}\n";
				index++;
			}
		}

		if (UIHandler.MultipleDates)
		{
			DocGenerator.GenerateRelationshipDoc();
		}
		else
		{
			DocGenerator.GenerateConsultationDoc();
		}
		GameObject.Find("location").GetComponent<TMP_Text>().text = "Файл сгенерирован";
	}

	public static void ClearPot()
	{
		for (int i = 1; i <= 6; i++)
		{
			GameObject.Find("e" + i).GetComponent<TMP_Text>().text = "";
		}
		GameObject.Find("2k").GetComponent<TMP_Text>().text = "";
		
		for (int i = 1; i <= 12; i++)
		{
			GameObject.Find("p" + i).GetComponent<TMP_Text>().text = "";
		}

		GameObject.Find("kz1").GetComponent<TMP_Text>().text = "";
		GameObject.Find("kz2").GetComponent<TMP_Text>().text = "";

		for (int i = 1; i <= 3; i++)
		{
			GameObject.Find("pr" + i).GetComponent<TMP_Text>().text = "";
		}
		
		GameObject.Find("shp1").GetComponent<TMP_Text>().text = "";
		GameObject.Find("shp2").GetComponent<TMP_Text>().text = "";

		GameObject.Find("location").GetComponent<TMP_Text>().text = "";
	}
}

public static class Helper
{
	public static int ApplyReduction(this int value)
	{
		return value > 22 ? (value - 22).ApplyReduction() : value;
	}
	
	public static string ToRoman(this int number)
	{
		if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
		if (number < 1) return string.Empty;            
		if (number >= 10) return "X" + ToRoman(number - 10);
		if (number >= 9) return "IX" + ToRoman(number - 9);
		if (number >= 5) return "V" + ToRoman(number - 5);
		if (number >= 4) return "IV" + ToRoman(number - 4);
		if (number >= 1) return "I" + ToRoman(number - 1);
		throw new ArgumentOutOfRangeException("something bad happened");
	}

	public static string ZeroTo22(this string number)
	{
		return number == string.Empty ? "XXII" : number;
	}
}