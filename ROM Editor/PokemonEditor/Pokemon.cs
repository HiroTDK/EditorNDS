using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROM_Editor.PokemonEditor
{
	class Pokemon
	{
		private string name;
		private string[] species;
		private string[] classification;
		private string[] flavour;
		private string[] height;
		private string[] weight;

        private byte baseHP;
        private byte baseAttack;
        private byte baseDefense;
        private byte baseSpeed;
        private byte baseSpecialAttack;
        private byte baseSpecialDefsne;

        private byte effortHP;
        private byte effortAttack;
        private byte effortDefense;
        private byte effortSpeed;
        private byte effortSpecialAttack;
        private byte effortSpecialDefense;

        private byte baseExperience;
        private byte baseHappiness;
        private byte catchRate;
        private byte genderRatio;
        private byte hatchMultiplier;
        private byte runChance;

        private byte type1;
        private byte type2;

        private byte ability1;
        private byte ability2;

        private byte eggGroup1;
        private byte eggGroup2;

        private byte growthRate;

        private byte color;

        private ushort babyID;

        private ushort heldItem1;
        private ushort heldItem2;

		private bool[] machines;
		private bool[] tutors;

        private List<short> eggMoves;

		struct learnsetEntry
		{
			ushort move;
			byte level;
		}

        private List<learnsetEntry> Learnset;

		struct evolutionEntry
		{
			byte method;
			ushort requirement;
			ushort evolution;
		}

        private List<evolutionEntry> evolutions;

        private byte shadowSize;
        private sbyte shadowOffeset;
        private sbyte offsetY;
        private sbyte offsetYMaleSingle;
        private sbyte offsetYMaleDouble;
        private sbyte offsetYFemaleSingle;
        private sbyte offsetYFemaleDouble;



        public Pokemon()
        {

        }
	}
}