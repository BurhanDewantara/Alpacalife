using UnityEngine;
using System.Collections;
using ScottGarland;

public enum EnvironmentType
{
	Sky,
	Ground,


	Mountain,
	Rainbow,
	Zeppelin,

	House,
	TreeL1,
	TreeL2,
	TreeB1,
	TreeB2,
	PondF,
	PondB,
	FenceL,
	FenceR,
	FenceB,
	Haysack,

}

public enum ThemeType{
	Classic
}

public class EnvironmentSO: ScriptableObject {

	public string name;
	public ThemeType theme;
	public EnvironmentType type;
	public int level;
	public Sprite sprite;


	public BigInteger upgradePrice;
	public BigInteger multiplier;


}
