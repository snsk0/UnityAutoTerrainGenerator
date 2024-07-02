using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UniPTG
{
    [FilePath("/UniPTGSettings.dat", FilePathAttribute.Location.ProjectFolder)]
    internal class MonoScriptDatabase : ScriptableSingleton<MonoScriptDatabase>
    {
        [SerializeField]
        private List<MonoScript> _noiseGenerators;

        [SerializeField]
        private List<MonoScript> _heightmapGenerators;

        internal IReadOnlyList<Type> GetHeightmapGeneratorTypes()
        {
            return _heightmapGenerators?.Select((script) => script.GetClass()).ToList().AsReadOnly();
        }

        internal IReadOnlyList<MonoScript> GetHeightmapGeneratorScripts()
        {
            return _heightmapGenerators?.AsReadOnly();
        }

        internal void Update()
        {
            //HeightmapGeneratorˆÈŠO‚ð”rœ‚·‚é
            _heightmapGenerators = _heightmapGenerators.Where((script) => script.GetClass().IsSubclassOf(typeof(HeightmapGeneratorBase))).ToList();

            Save(true);
        }
    }
}

