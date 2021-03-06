﻿using UnityEngine;
using System.Collections;
using ScottGarland;

public enum BigIntegerUnit
{
	None,
	K,
	M,
	B,
	T,
	Qu,
	Qi,
	Sx,
	Sp,
	Oc,
	No,
	De,
	aa,
	bb,
	cc,
	dd,
	ee,
	ff,
	gg,
	hh,
	ii,
	jj,
	kk,
	ll,
	mm,
	nn,
	oo,
	pp,
	qq,
	rr,
	ss,
	tt,
	uu,
	vv,
	ww,
	xx,
	yy,
	zz,
	Aa,
	Bb,
	Cc,
	Dd,
	Ee,
	Ff,
	Gg,
	Hh,
	Ii,
	Jj,
	Kk,
	Ll,
	Mm,
	Nn,
	Oo,
	Pp,
	Qq,
	Rr,
	Ss,
	Tt,
	Uu,
	Vv,
	Ww,
	Xx,
	Yy,
	Zz,
	AA,
	BB,
	CC,
	DD,
	EE,
	FF,
	GG,
	HH,
	II,
	JJ,
	KK,
	LL,
	MM,
	NN,
	OO,
	PP,
	QQ,
	RR,
	SS,
	TT,
	UU,
	VV,
	WW,
	XX,
	YY,
	ZZ,
}

public static class BigIntegerExtension
{
//	private BigInteger coin;	
	private const int THRESHOLD = 1000;

	public static BigIntegerUnit GetUnit(this BigInteger bigInteger)
	{
		BigIntegerUnit unit = BigIntegerUnit.None;
		BigInteger tempCoin = new BigInteger (bigInteger.ToString ());
		while (tempCoin > THRESHOLD) {
			unit++;
			tempCoin /= THRESHOLD;
		}
		return unit;
	}

	private static int GetLast3Digit(BigInteger bigInteger)
	{
		BigInteger tempCoin = new BigInteger (bigInteger.ToString ());
		int last3Digit = 0;
		while (tempCoin > THRESHOLD) {
			last3Digit = int.Parse((tempCoin % THRESHOLD).ToString());
			tempCoin /= THRESHOLD;
		}
		return last3Digit ;
	}
	private static int GetLast3Digit(int tempCoin)
	{
		int last3Digit = 0;
		while (tempCoin > THRESHOLD) {
			last3Digit = int.Parse((tempCoin % THRESHOLD).ToString());
			tempCoin /= THRESHOLD;
		}
		return last3Digit ;
	}

	public static string ToStringShort (this BigInteger bigInteger)
	{
		if (bigInteger == -1 ) return "MAX";
			 
		BigInteger tempCoin = new BigInteger (bigInteger.ToString ());
		int last3Digit = 0;
		while (tempCoin > THRESHOLD) {
			last3Digit = int.Parse((tempCoin % THRESHOLD).ToString());
			tempCoin /= THRESHOLD;
		}

		BigIntegerUnit unit = GetUnit (bigInteger);

		if(unit > BigIntegerUnit.None)
			return tempCoin.ToString () + "." + last3Digit.ToString("D3").Substring(0,2) + unit;
		return tempCoin.ToString ();
	}

	public static float DivideWith(this BigInteger bigInteger, BigInteger otherBigInteger,int coma = 4)
	{
		int multiplier = (int)Mathf.Pow(10,coma);
		BigInteger tempCoin = new BigInteger (bigInteger.ToString ());
		tempCoin *= multiplier;

		BigInteger sisa = tempCoin / otherBigInteger;
		float percent = float.Parse(sisa.ToString()) / multiplier ;

		return percent;

	}

	public static bool IsGreaterThan(this BigInteger bigInteger, int value, BigIntegerUnit unit)
	{
		if(GetUnit(bigInteger) < unit)
			return false;
		
		else if(GetUnit(bigInteger) > unit)
			return true;

		else if(GetLast3Digit(bigInteger) <= GetLast3Digit(value))
			return false;

		return true;

	}

}
