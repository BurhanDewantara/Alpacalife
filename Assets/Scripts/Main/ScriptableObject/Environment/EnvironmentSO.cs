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
	Tree1,
	Tree2,
	Tree3,
	Tree4,
	Pond,
	Fence,
	Fence2,


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
