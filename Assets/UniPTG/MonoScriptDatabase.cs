using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UniPTG
{
    [FilePath("ProjectSettings/UniPTGSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    internal class MonoScriptDatabase : ScriptableSingleton<MonoScriptDatabase>
    {
        [SerializeField]
        private List<MonoScript> _noiseGenerators;

        [SerializeField]
        private List<MonoScript> _heightmapGenerators;

        internal IReadOnlyList<Type> GetNoiseGeneratorTypes()
        {
            return _noiseGenerators?.Where((script) => script != null).Select((script) => script.GetClass()).ToList().AsReadOnly();
        }

        internal IReadOnlyList<MonoScript> GetNoiseGeneratorScripts()
        {
            return _noiseGenerators?.Where((script) => script != null).ToList().AsReadOnly();
        }

        internal IReadOnlyList<Type> GetHeightmapGeneratorTypes()
        {
            return _heightmapGenerators?.Where((script) => script != null).Select((script) => script.GetClass()).ToList().AsReadOnly();
        }

        internal IReadOnlyList<MonoScript> GetHeightmapGeneratorScripts()
        {
            return _heightmapGenerators?.Where((script) => script != null).ToList().AsReadOnly();
        }

        internal void Update()
        {
            //generatorˆÈŠOnull‘ã“ü
            for(int i = 0; i < _noiseGenerators.Count; i++)
            {
                if(_noiseGenerators[i] == null)
                {
                    continue;
                }

                if (!_noiseGenerators[i].GetClass().IsSubclassOf(typeof(NoiseGeneratorBase))) 
                {
                    _noiseGenerators[i] = null;
                }
            }

            for (int i = 0; i < _heightmapGenerators.Count; i++)
            {
                if (_heightmapGenerators == null)
                {
                    continue;
                }

                if (!_heightmapGenerators[i].GetClass().IsSubclassOf(typeof(HeightmapGeneratorBase)))
                {
                    _heightmapGenerators[i] = null;
                }
            }

            //‰i‘±‰»ˆ—
            Save(true);
        }
    }
}

