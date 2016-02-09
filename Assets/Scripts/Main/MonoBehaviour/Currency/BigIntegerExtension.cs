using UnityEngine;
using System.Collections;
using ScottGarland;

public enum BigIntegerUnit
{
	None,
	K,
	M,
	B,
	T,
	Q,
	P,
	V,
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
	aaa,
	bbb,
	ccc,
	ddd,
	eee,
	fff,
	ggg,
	hhh,
	iii,
	jjj,
	kkk,
	lll,
	mmm,
	nnn,
	ooo,
	ppp,
	qqq,
	rrr,
	sss,
	ttt,
	uuu,
	vvv,
	www,
	xxx,
	yyy,
	zzz,
	aaaa,
	bbbb,
	cccc,
	dddd,
	eeee,
	ffff,
	gggg,
	hhhh,
	iiii,
	jjjj,
	kkkk,
	llll,
	mmmm,
	nnnn,
	oooo,
	pppp,
	qqqq,
	rrrr,
	ssss,
	tttt,
	uuuu,
	vvvv,
	wwww,
	xxxx,
	yyyy,
	zzzz
}

public static class BigIntegerExtension
{
//	private BigInteger coin;	
	private const int THRESHOLD = 1000;

	private static BigIntegerUnit GetUnit(this BigInteger bigInteger)
	{
		BigIntegerUnit unit = BigIntegerUnit.None;
		BigInteger tempCoin = new BigInteger (bigInteger.ToString ());
		while (tempCoin > THRESHOLD) {
			unit++;
			tempCoin /= THRESHOLD;
		}
		return unit;
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
			return tempCoin.ToString () + "." + last3Digit.ToString("D3").Substring(0,2) + " " + unit;
		return tempCoin.ToString ();
	}


}
