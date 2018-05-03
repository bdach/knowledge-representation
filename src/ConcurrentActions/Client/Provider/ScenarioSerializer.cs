using System;
using System.IO;
using Client.Exception;
using Client.Global;
using Microsoft.Win32;
using XSerializer;

namespace Client.Provider
{
    /// <summary>
    /// Class user to serialize and deserialize <see cref="Scenario"/> instances.
    /// </summary>
    public class ScenarioSerializer
    {
        /// <summary>
        /// Default path which the open and save file dialogs will show.
        /// </summary>
        private readonly string _defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        /// <summary>
        /// Private instance of <see cref="Scenario"/> serializer.
        /// </summary>
        private readonly XmlSerializer<Scenario> _serializer;

        /// <summary>
        /// Initializes a new <see cref="ScenarioSerializer"/> instance.
        /// </summary>
        public ScenarioSerializer()
        {
            _serializer = new XmlSerializer<Scenario>();
        }

        /// <summary>
        /// Serializes given <see cref="Scenario"/> instance into a file with given name.
        /// </summary>
        /// <param name="scenario"><see cref="Scenario"/> instance to serialize.</param>
        /// <param name="filepath">Path to output file.</param>
        public void Serialize(Scenario scenario, string filepath)
        {
            if (!string.IsNullOrEmpty(filepath))
            {
                var writer = new StreamWriter(filepath);

                try
                {
                    _serializer.Serialize(writer, scenario);
                }
                catch (InvalidOperationException ex)
                {
                    throw new SerializationException("Serialization of the current scenario to specified file failed");
                }

                writer.Close();
            }
        }

        /// <summary>
        /// Deserializes a <see cref="Scenario"/> instance from a designated file.
        /// </summary>
        /// <param name="filepath">Path to input file.</param>
        /// <returns><see cref="Scenario"/> instance or null if <see cref="filepath"/> was null or empty.</returns>
        public Scenario Deserialize(string filepath)
        {
            Scenario scenario = null;

            if (!string.IsNullOrEmpty(filepath))
            {
                var reader = new StreamReader(filepath);

                try
                {
                    scenario = _serializer.Deserialize(reader);
                }
                catch (InvalidOperationException ex)
                {
                    throw new SerializationException("Deserialization of specified scenario file failed");
                }

                reader.Close();
            }

            return scenario;
        }

        /// <summary>
        /// Opens file dialog allowing user to select a location to save a file.
        /// </summary>
        /// <returns>Path to selected file.</returns>
        public string PromptSaveFile()
        {
            var isoDateTimeString =
                DateTime.UtcNow
                    .ToString("s", System.Globalization.CultureInfo.InvariantCulture)
                    .Replace("-", string.Empty)
                    .Replace(":", string.Empty);

            var dialog = new SaveFileDialog
            {
                FileName = $"scenario-{isoDateTimeString}Z.xml",
                InitialDirectory = _defaultPath,
                RestoreDirectory = true,
                Filter = "XML|*.xml",
                Title = "Save Scenario File"
            };

            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }

        /// <summary>
        /// Opens file dialog allowing user to select a file to be opened.
        /// </summary>
        /// <returns>Path to selected file.</returns>
        public string PromptOpenFile()
        {
            var dialog = new OpenFileDialog
            {
                InitialDirectory = _defaultPath,
                RestoreDirectory = true,
                Filter = "XML|*.xml",
                Title = "Open Scenario File"
            };

            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }
    }
}