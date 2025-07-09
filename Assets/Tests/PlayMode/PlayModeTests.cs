using System.Collections;
using System.Collections.Generic;
using Codes.animal;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;
public class PlayModeTests
{

    /*
     * // A Test behaves as an ordinary method
    [Test]
    public void PlayModeTestsSimplePasses()
    {
        // Use the Assert class to test conditions
        Assert.AreEqual(3, 1 + 2);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator PlayModeTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
     */
    private Model model;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        SceneManager.LoadScene("MainGame");
        yield return new WaitForSeconds(1f);

        var modelGO = GameObject.Find("Model");
        Assert.IsNotNull(modelGO, "Model GameObject not found in scene.");

        model = modelGO.GetComponent<Model>();
        Assert.IsNotNull(model, "Model component not found.");

        model.money = 10000;
    }

    [UnityTest]
    public IEnumerator timeChangesTest(){
        int kezdoOra = model.hour;
        yield return new WaitForSeconds(2f);
        int endOra = model.hour;

        Assert.AreNotEqual(kezdoOra, endOra);
        Debug.Log("Test result(timeChangesTest): PASSED");
    }

    [UnityTest]
    public IEnumerator moveTest()
    {
        model.buy("cheetah", new Vector2(0, 0));
        GameObject gepard = GameObject.Find("CheetahObject(Clone)");
        Vector2 position1 = gepard.transform.position;
        yield return new WaitForSeconds(1f);
        Vector2 position2 = gepard.transform.position;

        Assert.AreNotEqual(position2, position1);
        Debug.Log("Test result(moveTest): PASSED");
    }
    /*
    [UnityTest]
    public IEnumerator poacherVisibilityTest()
    {
        model.makePoacher();
        Poacher poacher = model.poachers[0];
        model.buy("camera", new Vector2(10, 10));
        poacher.obj.transform.position = new Vector2(10, 10);
        poacher.targetPosition = new Vector2(10.1f, 10.1f);
        yield return new WaitForFixedUpdate();

        Assert.AreEqual(poacher.visible, true);
        Debug.Log("Test result(poacherVisibilityTest): PASSED");
    }*/
    [UnityTest]
    public IEnumerator makePoacherTest()
    {
        int count1 = model.poachers.Count;
        model.makePoacher();
        int count2 = model.poachers.Count;

        Assert.AreEqual(count1 + 1, count2, "Poacher not found");
        Debug.Log("Test result(makePoacherTest): PASSED");
        return null;
    }
    [UnityTest]
    public IEnumerator detectRangerOrJeepTest()
    {
        model.buyRanger("id");
        Ranger ranger = model.rangers[0];
        ranger.obj.transform.position = new Vector2(10, 10);
        bool detect = model.detectRangerOrJeep(new Vector2(10, 10), 5);

        Assert.AreEqual(detect, true, "Ranger not found");
        Debug.Log("Test result(detectRangerOrJeepTest): PASSED");
        return null;
    }

    [UnityTest]
    public IEnumerator detectAnimalTest()
    {
        model.buy("crocodile", new Vector2(10, 10));
        AnimalGroup group = model.detectAnimal(Codes.animal.AnimalType.Crocodile, new Vector2(10, 10), 5);

        Assert.IsNotNull(group, "AnimalGroup not found");
        Debug.Log("Test result(detectAnimalTest): PASSED");
        return null;
    }
    [UnityTest]
    public IEnumerator detectPoacherTest()
    {
        model.makePoacher();
        Poacher poacher = model.poachers[0];
        poacher.obj.transform.position = new Vector2(10, 10);
        Poacher detect = model.detectPoacher(new Vector2(10, 10), 5);

        Assert.IsNotNull(detect, "Poacher not found");
        Debug.Log("Test result(detectPoacherTest): PASSED");
        return null;
    }
    [UnityTest]
    public IEnumerator detectAnyAnimalTest()
    {
        model.buy("crocodile", new Vector2(10, 10));
        AnimalGroup group = model.detectAnyAnimal(new Vector2(10, 10));

        Assert.IsNotNull(group, "AnimalGroup not found");
        Debug.Log("Test result(detectAnyAnimalTest): PASSED");
        return null;
    }
    [UnityTest]
    public IEnumerator canBuyTest()
    {
        model.money = 90;
        bool canBuy = model.canBuy("jeep");

        Assert.AreEqual(canBuy, false);
        Debug.Log("Test result(canBuyTest): PASSED");
        return null;
    }
    [UnityTest]
    public IEnumerator buyTest()
    {
        Debug.Log("buyTest: Starting");

        Assert.IsNotNull(model, "Model is null.");
        Assert.IsNotNull(model.plants, "model.plants is null.");

        int treeCount1 = 0;
        foreach (Plant plant in model.plants)
        {
            if (plant is Tree)
            {
                ++treeCount1;
            }
        }
        Debug.Log($"buyTest: treeCount1 = {treeCount1}");

        model.buy("tree", new Vector2(15, 15));
        Debug.Log("buyTest: buy() called");

        // V�runk 1 frame-et, hogy biztosan hozz�ad�djon az �j n�v�ny
        yield return null;

        Assert.IsNotNull(model.plants, "model.plants is null after buy().");

        int treeCount2 = 0;
        foreach (Plant plant in model.plants)
        {
            if (plant is Tree)
            {
                ++treeCount2;
            }
        }
        Debug.Log($"buyTest: treeCount2 = {treeCount2}");

        Assert.AreEqual(treeCount1 + 1, treeCount2, "Tree count did not increase as expected.");
        Debug.Log("Test result(buyTest): PASSED");
    }

    [UnityTest]
    public IEnumerator buyRangerTest()
    {
        int count1 = model.rangers.Count;
        model.buyRanger("id");
        int count2 = model.rangers.Count;

        Assert.AreEqual(count1 + 1, count2);
        Debug.Log("Test result(buyRangerTest): PASSED");
        return null;
    }
    
    [UnityTest]
    public IEnumerator sellRangerTest()
    {
        Debug.Log("sell ranger started");
        model.buyRanger("id");
        Debug.Log("buy ranger finished");
        int count1 = model.rangers.Count;
        Debug.Log("rangers count finished");
        yield return new WaitForSeconds(0.25f);
        Debug.Log(count1);
        model.sellRanger("id");
        int count2 = model.rangers.Count;
        Debug.Log(count2);

        Assert.AreEqual(count2 + 1, count1);
        Debug.Log("Test result(sellRangerTest): PASSED");
    }
    
    [UnityTest]
    public IEnumerator rangerTargetChangeTest()
    {
        model.buyRanger("id");
        Ranger ranger = model.rangers[0];
        int target1 = ranger.target;
        model.rangerTargetChange("id");
        int target2 = ranger.target;

        Assert.AreNotEqual(target1, target2);
        Debug.Log("Test result(rangerTargetChangeTest): PASSED");
        return null;
    }
    [UnityTest]
    public IEnumerator payCheckTest()
    {
        model.buyRanger("id");
        int money1 = model.money;
        model.payCheck();
        int money2 = model.money;

        Assert.Greater(money1, money2);
        Debug.Log("Test result(payCheckTest): PASSED");
        return null;
    }
    [UnityTest]
    public IEnumerator ClickingMenuButtonLoadsGameScene()
    {
        // 1. Load the main menu scene
        SceneManager.LoadScene("Menu");
        yield return new WaitForSeconds(1f); // Wait for the scene to load

        Debug.Log("Starting test: ClickingMenuButtonLoadsGameScene");

        // 2. Find the GameModel in the scene
        var menuManager = GameObject.Find("MenuManager");
        Assert.IsNotNull(menuManager, "Menumanager not found in scene.");

        // 3. Find the "Hard" difficulty button (by name or tag or other logic)
        var hardButtonGO = GameObject.Find("Hard"); // <- Replace with your button's actual name
        Assert.IsNotNull(hardButtonGO, "HardButton not found in scene.");

        var button = hardButtonGO.GetComponent<Button>();
        Assert.IsNotNull(button, "HardButton does not have a Button component.");

        // 4. Simulate a click
        button.onClick.Invoke();

        yield return new WaitForSeconds(1f); // wait a frame for logic to process

        // 5. Assert difficulty was set
        var model = GameObject.Find("Model");
        Assert.IsNotNull(model, "Model not found in scene.");

        Debug.Log("Test result(ClickingMenuButtonLoadsGameScene): PASSED");
    }

    [UnityTest]
    public IEnumerator ObjectsLoadInGame()
    {
        
        SceneManager.LoadScene("MainGame");
        yield return new WaitForSeconds(1f);

        Debug.Log("Starting test: ObjectsLoadInGame");

        var hill = GameObject.Find("HillObject(Clone)");
        Assert.IsNotNull(hill, "Hill not found in scene.");

        var river = GameObject.Find("RiverObject(Clone)");
        Assert.IsNotNull(river, "River not found in scene.");

        var pond = GameObject.Find("PondObject(Clone)");
        Assert.IsNotNull(pond, "Pond not found in scene.");

        var grass = GameObject.Find("GrassObject(Clone)");
        Assert.IsNotNull(grass, "Grass not found in scene.");

        Debug.Log("Test result(ObjectsLoadInGame): PASSED");
    }

    [UnityTest]
    public IEnumerator droneTest()
    {
        model.buy("drone", new Vector2(-10, 12));
        GameObject drone = GameObject.Find("DroneObject(Clone)");
        Vector2 position1 = drone.transform.position;
        yield return new WaitForSeconds(1f);
        Vector2 position2 = drone.transform.position;

        Assert.AreNotEqual(position2, position1);
        Debug.Log("Test result(droneTest): PASSED");
    }

    [UnityTest]
    public IEnumerator airballoonTest()
    {
        model.buy("airballoon", new Vector2(20,20));
        GameObject airballoon = GameObject.Find("AirballoonObject(Clone)");
        Vector2 position1 = airballoon.transform.position;
        yield return new WaitForSeconds(1f);
        Vector2 position2 = airballoon.transform.position;

        Assert.AreNotEqual(position2, position1);
        Debug.Log("Test result(airballoonTest): PASSED");
    }

    [UnityTest]
    public IEnumerator pathFindingTest()
    {
        model.buy("path", new Vector2(-1, 0));
        model.buy("path", new Vector2(-1, -1));
        model.buy("path", new Vector2(-1, -2));
        Assert.AreEqual(1, model.validPaths.Count);
        model.buy("path", new Vector2(-2, 0));
        model.buy("path", new Vector2(-2, -1));
        Assert.AreEqual(2, model.validPaths.Count);
        model.buy("path", new Vector2(-2, -2));
        Assert.AreEqual(4, model.validPaths.Count);
        Debug.Log("Test result(pathFindingTest): PASSED");

        return null;
    }

    [UnityTest]
    public IEnumerator jeepMoveTest()
    {
        if(model.validPaths.Count == 0)
        {
            model.buy("path", new Vector2(-1, 0));
            model.buy("path", new Vector2(-1, -1));
            model.buy("path", new Vector2(-1, -2));
        }
        model.buy("jeep", new Vector2(30, 30));
        model.setVisitorsWaiting(100);
        GameObject jeep = GameObject.Find("JeepObject(Clone)");
        yield return new WaitForSeconds(1f);
        Vector2 position1 = jeep.transform.position;
        yield return new WaitForSeconds(1f);
        Vector2 position2 = jeep.transform.position;

        Assert.AreNotEqual(position2, position1);
        Debug.Log("Test result(jeepMoveTest): PASSED");
    }
    
    [UnityTest]
    public IEnumerator sortingOrderChangeTest()
    {
        AnimalGroup crocodile = null;
        foreach (AnimalGroup item in model.animalGroups)
        {
            if(item.animalType == AnimalType.Crocodile)
            {
                crocodile = item; break;
            }
        }

        crocodile.obj.transform.position = new Vector2(50, 50);
        SpriteRenderer sr = crocodile.obj.GetComponent<SpriteRenderer>();
        sr.sortingOrder = 10;
        model.buy("tree", new Vector2 (51,51));
        crocodile.targetPosition = new Vector2(55, 55);
        yield return new WaitForSeconds(1f);

        Assert.AreEqual(0, sr.sortingOrder);
        Debug.Log("Test result(sortingOrderChangeTest): PASSED");
    }

    [UnityTest]
    public IEnumerator SatisfactionTest()
    {
        //Crocodile croco1 = new Crocodile(0);
        //Crocodile croco2 = new Crocodile(0);
        //Gepard gep1 = new Gepard(0);
        //Gepard gep2 = new Gepard(0);
        //Gepard gep3 = new Gepard(0);
        //Hippo hippo1 = new Hippo(0);
        //Hippo hippo2 = new Hippo(0);
        //Gazella gaz1 = new Gazella(0);

        AnimalGroup crocodiles = new AnimalGroup(new Vector2(0, 1), AnimalType.Crocodile);
        AnimalGroup gepards = new AnimalGroup(new Vector2(0, 2), AnimalType.Gepard);
        AnimalGroup hippos = new AnimalGroup(new Vector2(0, 3), AnimalType.Hippo);
        AnimalGroup gazellas = new AnimalGroup(new Vector2(0, 4), AnimalType.Gazella);

        //crocodiles.animals.Add(croco1);
        //crocodiles.animals.Add(croco2);    
        //gepards.animals.Add(gep1);
        //gepards.animals.Add(gep2);
        //gepards.animals.Add(gep3);         
        //hippos.animals.Add(hippo1);
        //hippos.animals.Add(hippo2);        
        //gazellas.animals.Add(gaz1);       

        model.animalGroups.Clear();
        model.animalGroups.Add(crocodiles);
        model.animalGroups.Add(gepards);
        model.animalGroups.Add(hippos);
        model.animalGroups.Add(gazellas);

        List<AnimalGroup> encounteredAnimals = new List<AnimalGroup>();
        encounteredAnimals.Add(crocodiles);  
        encounteredAnimals.Add(gepards);     
        encounteredAnimals.Add(hippos);      

        int satisfaction = model.calculateSatisfaction(encounteredAnimals);

        Assert.AreEqual(8, satisfaction);
        Debug.Log("Test result(SatisfactionTest): PASSED");

        return null;
    }
    [UnityTest]
    public IEnumerator TestAnimalMovement()
    {
        // Arrange
        var initialPosition = new Vector2(5, 5);
        var animalGroup = new AnimalGroup(initialPosition, AnimalType.Gazella);
        model.animalGroups.Add(animalGroup);
        animalGroup.obj = UnityEngine.Object.Instantiate(model.gazelleObject, initialPosition, Quaternion.identity);

        var targetPosition = new Vector2(10, 10);
        animalGroup.targetPosition = targetPosition;

        // Act - wait for movement
        yield return new WaitForSeconds(1f);

        // Assert
        Assert.AreNotEqual(initialPosition, animalGroup.obj.transform.position);
        Assert.Less(Vector2.Distance(animalGroup.obj.transform.position, targetPosition),
                    Vector2.Distance(initialPosition, targetPosition));
        Debug.Log("Test result(TestAnimalMovement): PASSED");
    }
    [UnityTest]
    public IEnumerator TestAnimalStarvation()
    {
        // Arrange
        var animalGroup = new AnimalGroup(Vector2.zero, AnimalType.Gazella);
        model.animalGroups.Add(animalGroup);
        animalGroup.obj = UnityEngine.Object.Instantiate(model.gazelleObject, Vector2.zero, Quaternion.identity);

        // Set hunger to critical level
        foreach (var animal in animalGroup.animals)
        {
            animal.Hunger = 90; // Almost starving
        }

        // Act - wait for starvation check
        yield return new WaitForSeconds(1.1f); // Slightly more than the 1s interval in AnimalTimeCoroutine

        // Assert
        Assert.IsTrue(animalGroup.animals.Count < 3, "Animals should have died from starvation");
        Debug.Log("Test result(TestAnimalStarvation): PASSED");
    }
    [UnityTest]
    public IEnumerator TestHerbivoreEatingPlant()
    {
        // Arrange
        var plantPosition = new Vector2(5, 5);
        var grass = new Grass(plantPosition);
        model.plants.Add(grass);
        grass.obj = UnityEngine.Object.Instantiate(model.grassObject, plantPosition, Quaternion.identity);

        var animalGroup = new AnimalGroup(new Vector2(5.5f, 5.5f), AnimalType.Gazella);
        model.animalGroups.Add(animalGroup);
        animalGroup.obj = UnityEngine.Object.Instantiate(model.gazelleObject, animalGroup.spawnPosition, Quaternion.identity);

        // Set hunger to make them eat
        foreach (var animal in animalGroup.animals)
        {
            animal.Hunger = 80; // Hungry enough to eat
        }

        // Act - wait for eating behavior
        yield return new WaitForSeconds(1.1f);

        // Assert
        Assert.IsTrue(grass.Value < 100, "Plant should have been eaten");
        Assert.IsTrue(animalGroup.animals[0].Hunger < 80, "Animal should have reduced hunger");
        Debug.Log("Test result(TestHerbivoreEatingPlant): PASSED");
    }
    [UnityTest]
    public IEnumerator TestCarnivoreHunting()
    {
        // Arrange
        var preyPosition = new Vector2(5, 5);
        var preyGroup = new AnimalGroup(preyPosition, AnimalType.Gazella);
        model.animalGroups.Add(preyGroup);
        preyGroup.obj = UnityEngine.Object.Instantiate(model.gazelleObject, preyPosition, Quaternion.identity);

        var predatorPosition = new Vector2(5.5f, 5.5f);
        var predatorGroup = new AnimalGroup(predatorPosition, AnimalType.Gepard);
        model.animalGroups.Add(predatorGroup);
        predatorGroup.obj = UnityEngine.Object.Instantiate(model.cheetahObject, predatorPosition, Quaternion.identity);

        // Set hunger to make them hunt
        foreach (var animal in predatorGroup.animals)
        {
            animal.Hunger = 80; // Hungry enough to hunt
        }

        // Act - wait for hunting behavior
        yield return new WaitForSeconds(1.1f);

        // Assert
        Assert.IsTrue(preyGroup.animals.Count < 3, "Prey should have been killed");
        Assert.IsTrue(predatorGroup.animals[0].Hunger < 80, "Predator should have reduced hunger");
        Debug.Log("Test result(TestCarnivoreHunting): PASSED");
    }
    [UnityTest]
    public IEnumerator TestAnimalMating()
    {
        // Arrange
        var position1 = new Vector2(5, 5);
        var group1 = new AnimalGroup(position1, AnimalType.Gazella);
        model.animalGroups.Add(group1);
        group1.obj = UnityEngine.Object.Instantiate(model.gazelleObject, position1, Quaternion.identity);

        var position2 = new Vector2(5.1f, 5.1f);
        var group2 = new AnimalGroup(position2, AnimalType.Gazella);
        model.animalGroups.Add(group2);
        group2.obj = UnityEngine.Object.Instantiate(model.gazelleObject, position2, Quaternion.identity);

        // Set animals to be well-fed and not thirsty
        foreach (var animal in group1.animals)
        {
            animal.Hunger = 20;
            animal.Thirst = 20;
        }
        foreach (var animal in group2.animals)
        {
            animal.Hunger = 20;
            animal.Thirst = 20;
        }

        // Act - wait for mating behavior
        yield return new WaitForSeconds(1.1f);

        // Assert
        Assert.IsTrue(model.animalGroups.Count > 2, "New animal group should have been created through mating");
        Debug.Log("Test result(TestAnimalMating): PASSED");
    }
    [UnityTest]
    public IEnumerator TestAnimalPanicBehavior()
    {
        // Arrange
        var animalPosition = new Vector2(5, 5);
        var animalGroup = new AnimalGroup(animalPosition, AnimalType.Gazella);
        model.animalGroups.Add(animalGroup);
        animalGroup.obj = UnityEngine.Object.Instantiate(model.gazelleObject, animalPosition, Quaternion.identity);

        var originalTarget = animalGroup.targetPosition;

        // Act - trigger panic
        animalGroup.Panicked = 3;
        yield return new WaitForSeconds(0.1f);

        // Assert
        Assert.AreEqual(originalTarget, animalGroup.targetPosition, "Panicked animals should not change direction");

        // Wait for panic to wear off
        yield return new WaitForSeconds(3.1f);
        Assert.AreEqual(0, animalGroup.Panicked, "Panic should have worn off");
        Debug.Log("Test result(TestAnimalPanicBehavior): PASSED");
    }
    [UnityTest]
    public IEnumerator TestAnimalGroupMerging()
    {
        // Arrange
        var position1 = new Vector2(5, 5);
        var group1 = new AnimalGroup(position1, AnimalType.Gazella);
        model.animalGroups.Add(group1);
        group1.obj = UnityEngine.Object.Instantiate(model.gazelleObject, position1, Quaternion.identity);
        group1.Age();
        group1.Age();
        group1.Age();
        var position2 = new Vector2(5.1f, 5.1f);
        var group2 = new AnimalGroup(position2, AnimalType.Gazella);
        model.animalGroups.Add(group2);
        group2.obj = UnityEngine.Object.Instantiate(model.gazelleObject, position2, Quaternion.identity);

        int initialGroupCount = model.animalGroups.Count;
        int initialTotalAnimals = group1.animals.Count + group2.animals.Count;

        // Act - trigger merge
        group1.MergeGroups(group2);
        group2.MergeGroups(group1);
        yield return null; // Wait one frame for merge to complete

        // Assert
        Assert.IsTrue(group2.Merged,  "Group2 should have merged into group 1!");
        Assert.AreEqual(initialTotalAnimals, model.animalGroups.Find(g => g == group1).animals.Count,
                       "Merged group should contain all animals");
        Debug.Log("Test result(TestAnimalGroupMerging): PASSED");
    }
    [UnityTest]
    public IEnumerator AnimalGroup_CanMateWhenConditionsMet()
    {
        AnimalGroup testGroup = new AnimalGroup(new Vector2(2,0), AnimalType.Gazella);
        // Arrange
        testGroup.animals.Add(new Gazella(0) { Hunger = 30, Thirst = 30 }); // Female
        testGroup.animals.Add(new Gazella(1) { Hunger = 30, Thirst = 30 }); // Male

        testGroup.Mate();
        Assert.IsFalse(testGroup.AbleToMate(), "Should not be able to mate under conditions!");
        var mockRandom = new TestRandom(80);
        testGroup.animals[0].Hunger = 80;
        testGroup.animals[1].Hunger = 80;
        Assert.IsFalse(testGroup.AbleToMate(), "Should not be able to mate under conditions!");

        Debug.Log("Test result(AnimalGroup_CanMateWhenConditionsMet): PASSED");
        yield return null;
    }

    [UnityTest]
    public IEnumerator AnimalGroup_Eat_DistributesFoodProperly()
    {
        AnimalGroup testGroup = new AnimalGroup(new Vector2(2, 0), AnimalType.Gazella);
        // Arrange
        testGroup.animals.Add(new Gazella(0) { Hunger = 50 });
        testGroup.animals.Add(new Gazella(1) { Hunger = 50 });
        int foodAmount = 60;

        // Act
        int leftover = testGroup.Eat(foodAmount);

        // Assert
        Assert.IsTrue(leftover <= 0, "There was not supposed to be any leftovers!"); // All food should be consumed
        foreach (var animal in testGroup.animals)
        {
            Assert.Greater(animal.Hunger, 50);
        }

        Debug.Log("Test result(AnimalGroup_Eat_DistributesFoodProperly): PASSED");
        yield return null;
    }

    [UnityTest]
    public IEnumerator AnimalGroup_Drink_RefreshesAllAnimals()
    {
        AnimalGroup testGroup = new AnimalGroup(new Vector2(2, 0), AnimalType.Gazella);
        // Arrange
        testGroup.animals.Add(new Gazella(0) { Thirst = 30 });
        testGroup.animals.Add(new Gazella(1) { Thirst = 40 });

        // Act
        testGroup.Drink();

        // Assert
        foreach (var animal in testGroup.animals)
        {
            Assert.AreEqual(100, animal.Thirst);
        }

        Debug.Log("Test result(AnimalGroup_Drink_RefreshesAllAnimals): PASSED");
        yield return null;
    }
    /*
    [UnityTest]
    public IEnumerator AnimalGroup_FatherForMerge_ReturnsCorrectDecision()
    {
        AnimalGroup testGroup = new AnimalGroup(new Vector2(2, 0), AnimalType.Gazella);
        AnimalGroup mergeGroup = new AnimalGroup(new Vector2(2, 0), AnimalType.Gazella);
        // Arrange - Make testGroup clearly superior
        testGroup.animals.Add(new Gazella(1) { Age = 10, Hunger = 20, Thirst = 20 });
        mergeGroup.animals.Add(new Gazella(1) { Age = 5, Hunger = 30, Thirst = 30 });

        // Act & Assert
        Assert.IsTrue(testGroup.FatherForMerge(mergeGroup));
        Assert.IsFalse(mergeGroup.FatherForMerge(testGroup));

        Debug.Log("Test result(AnimalGroup_FatherForMerge_ReturnsCorrectDecision): PASSED");
        yield return null;
    }
    */

    [UnityTest]
    public IEnumerator AnimalGroup_OnTerrain_AdjustsVisionAndSpeed()
    {
        AnimalGroup testGroup = new AnimalGroup(new Vector2(2, 0), AnimalType.Gazella);
        // Arrange
        var hill = new Hill(Vector2.zero);

        // Act
        bool result = testGroup.OnTerrain(hill);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(4, testGroup.Vision);
        Assert.AreEqual(0.5f, testGroup.Speed);
        Assert.IsTrue(testGroup.Terrain);

        Debug.Log("Test result(AnimalGroup_OnTerrain_AdjustsVisionAndSpeed): PASSED");
        yield return null;
    }

    [UnityTest]
    public IEnumerator AnimalGroup_OffTerrain_RestoresDefaultValues()
    {
        AnimalGroup testGroup = new AnimalGroup(new Vector2(2, 0), AnimalType.Gazella);
        // Arrange
        testGroup.OnTerrain(new Hill(Vector2.zero));

        // Act
        testGroup.OffTerrain();

        // Assert
        Assert.AreEqual(3, testGroup.Vision);
        Assert.AreEqual(1f, testGroup.Speed);
        Assert.IsFalse(testGroup.Terrain);

        Debug.Log("Test result(AnimalGroup_OffTerrain_RestoresDefaultValues): PASSED");
        yield return null;
    }
    private class TestRandom : System.Random
    {
        private readonly int _value;

        public TestRandom(int value)
        {
            _value = value;
        }

        public override int Next(int maxValue)
        {
            return _value;
        }
    }

}
