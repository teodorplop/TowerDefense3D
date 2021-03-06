﻿using Ingame.towers;
using Ingame.waves;
using System.IO;

public partial class GameResources {
	private const string DEFAULT_MOD_PATH = "Default";

	private const string WALLET_PATH = "Wallet";
	private const string TOWER_ATTRIBUTES_PATH = "TowerAttributes";
	private const string MONSTER_ATTRIBUTES_PATH = "MonsterAttributes";
	private const string UNIT_ATTRIBUTES_PATH = "UnitAttributes";
	private const string WAVES_PATH = "Waves";
	private const string SEND_MONSTERS_PATH = "SendMonsters";
	private const string COMBAT_PATH = "Combat";
	private const string STRINGS_PATH = "Strings";

	private static string modPath = DEFAULT_MOD_PATH;

	private static string GetPath(string mod, string root, string level, string file) {
		string path = Path.Combine(Path.Combine(mod, root), string.Format("{0}_{1}", file, level));
		return path;
	}
	private static string GetDefaultPath(string mod, string root, string file) {
		string path = Path.Combine(Path.Combine(mod, root), file);
		return path;
	}

	private static T LoadJSON<T>(string root, string level, string file) where T : IGameResource {
		string path = GetPath(modPath, root, level, file);
		T t = LoadJSON<T>(path);
		if (t == null) {
			path = GetDefaultPath(modPath, root, file);
			t = LoadJSON<T>(path);

			if (t == null) {
				path = GetPath(DEFAULT_MOD_PATH, root, level, file);
				t = LoadJSON<T>(path);

				if (t == null) {
					path = GetDefaultPath(DEFAULT_MOD_PATH, root, file);
					t = LoadJSON<T>(path);
				}
			}
		}
		return t;
	}
	private static T LoadCSV<T>(string root, string level, string file) where T : CSVLoader {
		string path = GetPath(modPath, root, level, file);
		T t = LoadCSV<T>(path);
		if (t == null) {
			path = GetDefaultPath(modPath, root, file);
			t = LoadCSV<T>(path);

			if (t == null) {
				path = GetPath(DEFAULT_MOD_PATH, root, level, file);
				t = LoadCSV<T>(path);

				if (t == null) {
					path = GetDefaultPath(DEFAULT_MOD_PATH, root, file);
					t = LoadCSV<T>(path);
				}
			}
		}
		return t;
	}
	private static T LoadFile<T>(string root, string level, string file) where T : ILoadableFile, new() {
		string path = GetPath(modPath, root, level, file);
		T t = LoadFile<T>(path);
		if (t == null) {
			path = GetDefaultPath(modPath, root, file);
			t = LoadFile<T>(path);

			if (t == null) {
				path = GetPath(DEFAULT_MOD_PATH, root, level, file);
				t = LoadFile<T>(path);

				if (t == null) {
					path = GetDefaultPath(DEFAULT_MOD_PATH, root, file);
					t = LoadFile<T>(path);
				}
			}
		}
		return t;
	}

	public static void SetMod(string mod) {
		modPath = mod;
	}

	public static SendMonstersList LoadSendMonsters(string level) {
		return LoadJSON<SendMonstersList>(SEND_MONSTERS_PATH, level, "SendMonsters");
	}

	public static SendMonstersList LoadSendMonsters() {
		return LoadSendMonsters(SceneLoader.ActiveScene);
	}

	public static Wallet LoadWallet(string level) {
		return LoadJSON<Wallet>(WALLET_PATH, level, "Wallet");
	}
	public static Wallet LoadWallet() {
		return LoadWallet(SceneLoader.ActiveScene);
	}

	public static TowerAttributes LoadTowerAttributes(string level, string tower) {
		return LoadJSON<TowerAttributes>(TOWER_ATTRIBUTES_PATH, level, tower);
	}
	public static TowerAttributes LoadTowerAttributes(string tower) {
		return LoadTowerAttributes(SceneLoader.ActiveScene, tower);
	}

	public static UnitAttributes LoadMonsterAttributes(string level, string monster) {
		return LoadJSON<UnitAttributes>(MONSTER_ATTRIBUTES_PATH, level, monster);
	}
	public static UnitAttributes LoadMonsterAttributes(string monster) {
		return LoadMonsterAttributes(SceneLoader.ActiveScene, monster);
	}

	public static UnitAttributes LoadUnitAttributes(string level, string unit) {
		return LoadJSON<UnitAttributes>(UNIT_ATTRIBUTES_PATH, level, unit);
	}
	public static UnitAttributes LoadUnitAttributes(string unit) {
		return LoadUnitAttributes(SceneLoader.ActiveScene, unit);
	}

	public static Waves_Data LoadWaves(string level) {
		return LoadFile<Waves_Data>(WAVES_PATH, level, "Waves");
	}
	public static Waves_Data LoadWaves() {
		return LoadWaves(SceneLoader.ActiveScene);
	}

	public static Combat_Data LoadCombat(string level) {
		return LoadCSV<Combat_Data>(COMBAT_PATH, level, "Combat");
	}

	public static Combat_Data LoadCombat() {
		return LoadCombat(SceneLoader.ActiveScene);
	}

	public static Strings_Data LoadStrings(string language) {
		return LoadJSON<Strings_Data>(GetDefaultPath(DEFAULT_MOD_PATH, STRINGS_PATH, language));
	}
}
