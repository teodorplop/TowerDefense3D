﻿using System;
using UnityEngine;
using Ingame.waves;
using Ingame.towers;
using System.Collections;
using UnityEngine.SceneManagement;

public partial class GameManager : StateMachineBase {
	public enum GameState {
		None,
		Loading,
		Idle,
		TowerSelected,
		RallyPoint,
		GameEnded
	}

	private static GameManager _instance;
	public static GameManager Instance { get { return _instance; } }

	[SerializeField]
	private LayerMask _towerMask;
	[SerializeField]
	private LayerMask _unitMask;
	[SerializeField]
	private float _gridNodeRadius = 0.75f;
	[SerializeField]
	private int _gridBlurSize = 3;
	
	private UIManager _uiManager;
	private InputManager _inputManager;
	private WavesManager _wavesManager;
	private TowerFactory _towerFactory;
	private MonsterFactory _monsterFactory;
	private UnitFactory _unitFactory;
	private PathsContainer _pathsContainer;
	private RequestDispatcher _dispatcher;

	private HighlightManager _highlightManager;

	protected override void Awake() {
		base.Awake();

		_instance = this;
		_inputManager = FindObjectOfType<InputManager>();
		_wavesManager = FindObjectOfType<WavesManager>();
		_towerFactory = FindObjectOfType<TowerFactory>();
		_monsterFactory = FindObjectOfType<MonsterFactory>();
		_unitFactory = FindObjectOfType<UnitFactory>();
		_pathsContainer = FindObjectOfType<PathsContainer>();

		_dispatcher = new RequestDispatcher();

		_highlightManager = FindObjectOfType<HighlightManager>();

		InitializeHandlers();
	}
	void OnDestroy() {
		Players.OnDestroy();
		_instance = null;
	}

	void Start() {
		if (SceneManager.sceneCount == 1)
			StartLoading(StartMatch);
	}

	private void SetState(GameState state) {
		if (currentState == null || (GameState)currentState != state) {
			_inputManager.PopContext();
			_dispatcher.SetHandlers(None_ActionHandlers);

			_stateMachineHandler.SetState(state, this);

			// These should be performed only after we entered state. But meh... Wharever.
			_inputManager.PushContext(GenerateInputContext(state));
			_dispatcher.SetHandlers(GetHandlers(state));
		}
	}

	private InputContext GenerateInputContext(GameState state) {
		InputContext input = new InputContext();

		input.onMouseDown = ConfigureDelegate<Action<int, Vector3>>(state, "HandleMouseDown", None_HandleMouseDown);
		input.onMouseUp = ConfigureDelegate<Action<int, Vector3>>(state, "HandleMouseUp", None_HandleMouseUp);
		input.onMouse = ConfigureDelegate<Action<Vector3>>(state, "HandleMouse", None_HandleMouse);
		input.onKey = ConfigureDelegate<Action>(state, "HandleKey", None_HandleKey);

		return input;
	}

	private ActionHandler[] GetHandlers(GameState state) {
		return ConfigureField(state, "ActionHandlers", None_ActionHandlers);
	}

	private void InitializeHandlers() {
		ActionHandler sendMonsters = new SendMonstersHandler();
		ActionHandler sendMonster = new SendMonsterHandler();
		ActionHandler upgradeTower = new UpgradeTowerHandler();
		ActionHandler sellTower = new SellTowerHandler();

		Idle_ActionHandlers = new ActionHandler[] { sendMonsters, sendMonster, upgradeTower, sellTower };
		TowerSelected_ActionHandlers = new ActionHandler[] { sendMonsters, sendMonster, upgradeTower, sellTower };
	}

	#region PUBLICS
	public void MonsterDied(Monster monster) {
		monster.owner.Wallet.Add(Wallet.Currency.Gold, monster.GoldAwarded);
		_uiManager.Refresh();
	}
	public void SetRallyPoint(Tower tower) {
		_rallyPointTower = tower;
		SetState(GameState.RallyPoint);
	}
	#endregion
}
