using UnityEngine;
using System.Collections;

[System.Serializable]
public enum InfCurrencyUnit
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

public class InfCurrency 
{
	
	private BigInt coin;	
	private InfCurrencyUnit unit;
	public InfCurrencyUnit Unit{
		set{
			unit = value;
		}
		get{ 

			if(coin != null && (unit != (InfCurrencyUnit) coin.Count-1))
					return (InfCurrencyUnit) coin.Count-1;

			return unit;
		}
	}
	// Use this for initialization
	public InfCurrency()
	{


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
