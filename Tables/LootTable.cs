using Godot;
using System;
using System.Collections.Generic;

public class LootTable<T>
{
    Random rand = new Random();
    public TableList<T, double> lootTable;

    public double totalTableValue;
    public LootTable()
    {
        lootTable = new TableList<T, double>();
    }

    public LootTable(List<T> itemList, List<double> chanceList)
    {
        lootTable = new TableList<T, double>();

        if(itemList.Count != chanceList.Count)
        {
            throw new Exception("Item List and Chance List of Loot Table must be the same length");
        }

        for (int i = 0; i < itemList.Count; i++)
        {
            Add(itemList[i], chanceList[i]);
        }
    }

    public T Get(int index)
    {
        return lootTable.GetLeft(index);
    }

    public void Add(T item, double chance)
    {
        totalTableValue += chance;
        lootTable.Add(item, chance);
    }

    /// <summary>
    /// "D&D" or other die roll style RPG loot table
    /// 
    /// It adds up all the chances and places them in a range from 0-MAX
    /// 
    /// which you roll a dMAX and get 1 item from the table.
    /// 
    /// For example you might have a table that looks like:
    /// 
    /// Bones: 5
    /// Scales: 5
    /// Tail: 2
    /// Ancient Fairy Dust of Doom: 1
    /// 
    /// Note this can be more or less than 100 but that's okay
    /// It's not inherently a percentage.
    /// 
    /// In this case you'd roll rand.Next(0, 13)
    /// 
    /// and then check each subsequent percent. So a 7 would get scales
    /// since the numbers represent item hit "ranges"
    /// 
    /// Bones: 7 > 5
    /// Scales: 7 > 10  <= since this is first in the linear list pick this
    /// Tail: 7 > 12
    /// Ancient Fairy Dust of Doom: 7 > 13
    /// 
    /// If you roll 3 times from this table you in theory could get really
    /// Lucky and roll 3 Fairy Dusts, but would require you getting 3
    /// 13s in a row.
    /// 
    /// </summary>
    /// <param name="rollCount">How many times to roll</param>
    /// <returns>A list of items to get from the loot table</returns>
    public List<T> GetRandomLinear(int rollCount = 1)
    {
        List<T> itemsFromLoot = new List<T>();
        double totalPercents = 0;
        double roll = rand.NextDouble();
        foreach (var lootRoll in lootTable)
        {
            totalPercents += (double)lootRoll.Right;
        }

        //roll multiple times
        for (int i = 0; i < rollCount; i++)
        {
            double currentMin = 0;
            foreach (var lootRoll in lootTable)
            {
                //if it's in the range, add it to the list and break
                if (roll > lootRoll.Right)
                {
                    itemsFromLoot.Add(lootRoll.Left);
                    break;
                }
            }
        }

        return itemsFromLoot;
    }


    /// <summary>
    /// Xenoblade sytle table where it rolls a percent against each item
    /// and adds it. 
    /// 
    /// For example you might have a table like:
    /// 
    /// Bones: 80
    /// Bones: 50
    /// Bones: 30
    /// Scales: 50
    /// Scales: 20
    /// Tail: 10
    /// Ancient Fairy Dust of Doom: 5
    /// 
    /// In this case it would roll a number against each one
    /// a chance to drop up to a total of 7 items, but more 
    /// likely just 1-3ish
    /// 
    /// In this style of table, the input values should be a
    /// percentage as a 100 or higher would essentially be a guaranteed drop
    /// 
    /// Unlike the linear table, there is no chance to get any row in the table
    /// more than once, unless the item is on the table several times.
    /// </summary>
    /// <returns>A list of all the dropped items</returns>
    public List<T> GetRandomFullTable()
    {
        List<T> itemsFromLoot = new List<T>();
        foreach (var lootRoll in lootTable)
        {
            if ((rand.NextDouble() * 100.0) < lootRoll.Right)
            {
                itemsFromLoot.Add(lootRoll.Left);
            }
        }
        return itemsFromLoot;
    }
}