using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;


public enum BattleState
{
    Player,
    Enemys,
    noFight,
}

public enum facing
{
    up,
    down,
    left,
    right,
}

public enum result
{
    win,
    lose,
    neutral,
}

public class BattleManager : MonoBehaviour
{
    class BattleParticipant
    {
        public string nameChar;
        public int health;
        public int maxHealth;
        public int mAttack;
        public int defense;
        public int evade;
        public int speed;
        public List<skills> skillset;

        public BattleParticipant(BattleStats data)
        {
            nameChar = data.nameChar;
            health = data.maxHealth;
            maxHealth = data.maxHealth;
            defense = data.defense;
            mAttack = 1;
            evade = data.evade;
            speed = data.speed;
            skillset = new List<skills>();
            foreach (skills skill in data.Skills)
            {
                skillset.Add(new skills(skill));
            }
            skillset = skillset.OrderByDescending(x => x.priority).ToList();
        }
    }

    public BattleState battleState;
    public BattleStats playerStats;

    BattleParticipant player;
    [SerializeField] private List<BattleParticipant> enemies;
    [SerializeField] private GameObject BattleUI;
    [SerializeField] private GameObject skillPanel;
    [SerializeField] private List<GameObject> skillObj;
    [SerializeField] private List<GameObject> enemyPosition;
    [SerializeField] private List<GameObject> highlightTarget;
    [SerializeField] private GameObject playerPosition;
    [SerializeField] private GameObject healthPlayerUI;
    [SerializeField] private TextMeshProUGUI healthTextPlayer;
    [SerializeField] private Slider healthPlayerSlider;

    [SerializeField] private facing face;
    private result resultBattle;
    GameObject textBattle;
    GameObject buttonBattle;

    private Vector2 currentPlayerPosition;
    private int selectedTargetIndex = 0;
    private int selectedSkillIndex = 0;
    private bool changeCheck = false;
    private bool canClickSkill = true;
    private bool chooseTarget = false;
    private bool enemyTurnInProgress = false;
    private Coroutine skillCoroutine;
    private static BattleManager instance;

    public static BattleManager GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one instance of BattleManager found!");
        }
        instance = this;
        textBattle = BattleUI.transform.Find("BottomPanel(Text)").gameObject;
        buttonBattle = BattleUI.transform.Find("BottomPanel(PlayerButton)").gameObject;
        skillCoroutine = null;
    }

    void Update()
    {
        if (chooseTarget)
        {
            if (InputManager.GetInstance().GetMoveDirection() != Vector2.zero && !changeCheck)
            {
                changeCheck = true;
                if(InputManager.GetInstance().GetMoveDirection().x > 0)
                {
                    StartCoroutine(changeSelect(1));
                }
                else if (InputManager.GetInstance().GetMoveDirection().x < 0)
                {
                    StartCoroutine(changeSelect(-1));
                }
                return;
            }
            if(InputManager.GetInstance().GetSelectPressed() && skillCoroutine == null)
            {
                chooseTarget = false;
                skillPanel.SetActive(false);
                //buttonBattle.SetActive(true);
                PlayerTurn(selectedSkillIndex, selectedTargetIndex);
                highlightTarget[selectedTargetIndex].SetActive(false);
                canClickSkill = true;
            }
            return;
        }
        if (battleState == BattleState.noFight)
        {
            return;
        }
        if(battleState == BattleState.Enemys && !enemyTurnInProgress)
        {
            //Debug.Log("debug enemy turn");
            enemyTurnInProgress = true;
            StartCoroutine(enemyTurn());
        }
    }

    public void StartBattle(BattleStats[] enemyStats, List<GameObject> enemyPositions, GameObject playerPos)
    {
        resultBattle = result.neutral;
        selectedTargetIndex = 0;
        skillPanel.SetActive(false);
        chooseTarget = false;
        canClickSkill = true;
        changeCheck = false;
        enemyTurnInProgress = false;
        enemies = new List<BattleParticipant>();
        enemyPosition = new List<GameObject>();
        highlightTarget = new List<GameObject>();
        currentPlayerPosition = new Vector2(PlayerOverworld.GetInstance().transform.position.x, PlayerOverworld.GetInstance().transform.position.y);
        PlayerOverworld.GetInstance().transform.position = playerPos.transform.position;
        switch (face)
        {
            case facing.up:
                PlayerOverworld.GetInstance().FaceUp();
                break;
            case facing.down:
                PlayerOverworld.GetInstance().FaceDown();
                break;
            case facing.right:
                PlayerOverworld.GetInstance().FaceRight();
                break;
            case facing:
                PlayerOverworld.GetInstance().FaceLeft();
                break;
            default:

        }
        player = new BattleParticipant(playerStats);
        healthPlayerSlider.maxValue = player.maxHealth;
        healthPlayerSlider.value = player.health;
        healthTextPlayer.text = $"{player.health}/{player.maxHealth}";
        int index = 0;
        foreach (BattleStats enemyStat in enemyStats)
        {
            BattleParticipant enemy = new BattleParticipant(enemyStat);
            enemyPosition.Add(Instantiate(enemyStat.prefab, enemyPositions[index].transform.position, Quaternion.identity));
            highlightTarget.Add(enemyPositions[index].transform.Find("select").gameObject);
            enemies.Add(enemy);
            index++;
        }
        StartCoroutine(delayShow());
    }

    private IEnumerator delayShow()
    {
        battleState = BattleState.Player;
        BattleUI.SetActive(true);
        buttonBattle.SetActive(false);
        textBattle.SetActive(true);
        textBattle.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Enemy Approaching...";
        yield return new WaitForSeconds(2f);
        textBattle.SetActive(false);
        buttonBattle.SetActive(true);
    }

    public void EndBattle()
    {
        StartCoroutine(resultShow());
        battleState = BattleState.noFight;
    }

    private IEnumerator resultShow()
    {
        yield return new WaitUntil(() => skillCoroutine == null);
        textBattle.SetActive(true);
        buttonBattle.SetActive(false);
        if (resultBattle == result.win)
        {
            textBattle.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "You Win!";
        }
        else if (resultBattle == result.lose)
        {
            textBattle.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "You Lose!";
        }
        yield return new WaitForSeconds(2f);
        for(int i = 0; i < enemyPosition.Count; i++)
        {
            Destroy(enemyPosition[i]);
        }
        BattleUI.SetActive(false);
        PlayerOverworld.GetInstance().transform.position = currentPlayerPosition;
        PlayerOverworld.GetInstance().isFreeze = false;
        currentPlayerPosition = Vector2.zero;
    }

    private IEnumerator enemyTurn()
    {
        //Debug.Log("Enemy turn!");
        bool checkEnemyAlive = false;
        enemies = enemies.OrderByDescending(x => x.speed).ToList();
        foreach (BattleParticipant enemy in enemies)
        {
            if(enemy.health <= 0)
            {
                continue;
            }
            checkEnemyAlive = true;
            if (enemy.speed == 0)
            {
                enemy.speed = 1;
            }
            else
            {
                bool useSkill = false;
                foreach (skills skill in enemy.skillset)
                {
                    if (skill.cooldownRemaining <= 0 && !useSkill)
                    {
                        switch (skill.type)
                        {
                            case SkillType.Attack:
                                {
                                    //Debug.Log("enemy used attack");
                                    useSkill = true;
                                    skillCoroutine = StartCoroutine(useSkillUI(skill.name, enemy.nameChar));
                                    PlayerGetDamage(skill.damage, skill.accuracy);
                                    break;
                                }
                            case SkillType.Heal:
                                {
                                    foreach (BattleParticipant ally in enemies)
                                    {
                                        if (ally.health < ally.maxHealth && ally.health < (ally.maxHealth*70/100))
                                        {
                                            useSkill = true;
                                            skillCoroutine = StartCoroutine(useSkillUI(skill.name, enemy.nameChar));
                                            ally.health += skill.heal;
                                            if (ally.health > ally.maxHealth)
                                            {
                                                ally.health = ally.maxHealth;
                                            }
                                            break;
                                        }
                                    }
                                    break;
                                }
                            case SkillType.Evade:
                                {
                                    // Handle evade skill
                                    break;
                                }
                            default: break;
                        }
                        if (useSkill) {
                            skill.cooldownRemaining = skill.cooldown;
                        }
                    }
                    skill.cooldownRemaining -= 1;
                }
                enemy.speed += 1;
                yield return new WaitUntil(() => skillCoroutine == null);
            }
        }
        if (!checkEnemyAlive)
        {
            resultBattle = result.win;
            EndBattle();
        }
        else
        {
            StartCoroutine(changeState());
        }
        yield return null;
    }

    private IEnumerator changeState()
    {
        yield return new WaitUntil(() => skillCoroutine == null);
        yield return new WaitForSeconds(0.5f);
        if (battleState == BattleState.Player)
        {
            textBattle.SetActive(true);
            buttonBattle.SetActive(false);
            battleState = BattleState.Enemys;
            enemyTurnInProgress = false;
        }
        else if (battleState == BattleState.Enemys)
        {
            //Debug.Log("test change");
            textBattle.SetActive(false);
            buttonBattle.SetActive(true);
            battleState = BattleState.Player;
        }
    }
    private IEnumerator useSkillUI(string skillName, string ownerName)
    {
        buttonBattle.SetActive(false);
        textBattle.SetActive(true);
        textBattle.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"{ownerName} used {skillName}!";
        yield return new WaitForSeconds(1.5f);
        skillCoroutine = null;
    }
    
    public void PlayerTurn(int moveIndex, int targetIndex)
    {
        if (battleState != BattleState.Player || player.speed == -1)
        {
            Debug.Log("It's not the player's turn!");
            return;
        }
        BattleParticipant target = enemies[targetIndex];
        PlayerAttack(target, moveIndex);
    }

    public void showSkill()
    {
        buttonBattle.SetActive(false);
        textBattle.SetActive(true);
        textBattle.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Choose a skill!";
        skillPanel.SetActive(true);
        int index = 0;
        foreach (var skill in player.skillset)
        {
            skillObj[index].SetActive(true);
            GameObject coolDownPanel = skillObj[index].transform.Find("CooldownPanel").gameObject;
            if (skill.cooldownRemaining <= 0)
            {
                coolDownPanel.SetActive(false);
            }
            else
            {
                coolDownPanel.SetActive(true);
                coolDownPanel.transform.Find("CooldownCount").GetComponent<TextMeshProUGUI>().text = skill.cooldownRemaining.ToString();
            }
            index++;
        }
        if(index < skillObj.Count)
        {
            for (int i = index; i < skillObj.Count; i++)
            {
                skillObj[i].SetActive(false);
            }
        }
    }

    public void afterSkill(int moveIndex)
    {
        selectedSkillIndex = moveIndex;
        if (player.skillset[moveIndex].cooldownRemaining > 0)
        {
            Debug.Log("Skill is on cooldown!");
            return;
        }
        if (!canClickSkill) return;
        canClickSkill = false;
        showTarget();
    }

    void PlayerAttack(BattleParticipant target, int moveIndex)
    {
        skillCoroutine = StartCoroutine(useSkillUI(player.skillset[moveIndex].name, "???"));
        player.skillset[moveIndex].cooldownRemaining = player.skillset[moveIndex].cooldown;
        int checkAcc = Random.Range(target.evade, 100);
        int playerAcc = Random.Range(player.skillset[moveIndex].accuracy, 100);
        //Debug.Log($"Player accuracy: {playerAcc}, Target evade: {checkAcc}");
        if (playerAcc < checkAcc)
        {
            StartCoroutine(attackMiss());
            StartCoroutine(changeState());
            foreach (var skill in player.skillset)
            {
                if (skill.cooldownRemaining > 0)
                {
                    skill.cooldownRemaining--;
                }
            }
            return;
        }
        target.health -= player.skillset[moveIndex].damage;
        //Debug.Log($"Target health after attack: {target.health}");
        if (target.health <= 0)
        {
            target.health = 0;
            Debug.Log("Enemy defeated!");
        }
        foreach (var skill in player.skillset)
        {
            if (skill.cooldownRemaining > 0)
            {
                skill.cooldownRemaining--;
            }
        }
        StartCoroutine(changeState());
    }

    public void PlayerGetDamage(int damage, int accuracy)
    {
        int checkAcc = Random.Range(player.evade, 100);
        int enemyAcc = Random.Range(accuracy, 100);
        if (enemyAcc < checkAcc && enemyAcc != checkAcc)
        {
            StartCoroutine(attackMiss());
            return;
        }
        //Debug.Log("Attack hit!");
        player.health -= damage;
        healthPlayerSlider.value = (float)player.health;
        healthTextPlayer.text = $"{player.health}/{player.maxHealth}";
        Debug.Log($"Player health after attack: {player.health}");
        if (player.health <= 0)
        {
            player.health = 0;
            Debug.Log("Player defeated!");
            resultBattle = result.lose;
            EndBattle();
        }
    }

    private IEnumerator attackMiss()
    {
        yield return new WaitUntil(() => skillCoroutine == null);
        textBattle.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Attack missed!";
    }

    private void showTarget()
    {
        //Debug.Log("Choose a target!");
        chooseTarget = true;
        canClickSkill = false;
        highlightTarget[selectedTargetIndex].SetActive(true);
    }


    private IEnumerator changeSelect(int index)
    {
        highlightTarget[selectedTargetIndex].SetActive(false);
        if (index + selectedTargetIndex >= 0 && index + selectedTargetIndex < enemies.Count)
        {
            selectedTargetIndex += index;
        }
        highlightTarget[selectedTargetIndex].SetActive(true);
        yield return new WaitForSeconds(0.2f);
        changeCheck = false;
    }

}