using System;
using System.IO;
using System.Linq;
using Client.DataTransfer;
using Client.Exception;
using Microsoft.Win32;

namespace Client.Provider
{
    /// <summary>
    /// Class user to serialize and deserialize <see cref="GrammarInput"/> instances.
    /// </summary>
    public class GrammarSerializer
    {
        /// <summary>
        /// Default path which the open and save file dialogs will show.
        /// </summary>
        private readonly string _defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        /// <summary>
        /// Array of action domain keywords.
        /// </summary>
        private static readonly string[] ActionDomainKeyWords = { "causes", "impossible", "releases", "always", "noninertial", "initially", "observable", "after" };

        /// <summary>
        /// Array of query set keywords.
        /// </summary>
        private static readonly string[] QuerySetKeyWords = { "accessible", "executable", "possibly", "necessary" };

        /// <summary>
        /// Serializes given <see cref="GrammarInput"/> instance into a file with given name.
        /// </summary>
        /// <param name="grammarInput"><see cref="GrammarInput"/> instance to serialize.</param>
        /// <param name="filepath">Path to output file.</param>
        /// <exception cref="SerializationException">Thrown when serialization of provided <see cref="GrammarInput"/> instance failed.</exception>
        public void Serialize(GrammarInput grammarInput, string filepath)
        {
            if (grammarInput != null && !string.IsNullOrEmpty(filepath))
            {
                var writer = new StreamWriter(filepath);

                try
                {
                    writer.Write(grammarInput.ActionDomainInput);
                    writer.Write(Environment.NewLine);
                    writer.Write(grammarInput.QuerySetInput);
                }
                catch (InvalidOperationException)
                {
                    throw new SerializationException("GrammarSerializationFailed");
                }

                writer.Close();
            }
        }

        /// <summary>
        /// Deserializes a <see cref="GrammarInput"/> instance from a designated file.
        /// </summary>
        /// <param name="filepath">Path to input file.</param>
        /// <returns><see cref="GrammarInput"/> instance or null if <see cref="filepath"/> was null or empty.</returns>
        /// <exception cref="SerializationException">Thrown when deserialization of <see cref="GrammarInput"/> instance from provided file failed.</exception>
        public GrammarInput Deserialize(string filepath)
        {
            GrammarInput grammarInput = null;

            if (!string.IsNullOrEmpty(filepath))
            {
                var reader = new StreamReader(filepath);

                string actionDomainInput = null;
                string queryInput = null;

                try
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (QuerySetKeyWords.Any(x => line.Contains(x)))
                        {
                            queryInput = string.Concat(queryInput, line, Environment.NewLine);
                        }
                        else if (ActionDomainKeyWords.Any(x => line.Contains(x)))
                        {
                            actionDomainInput = string.Concat(actionDomainInput, line, Environment.NewLine);
                        }
                    }
                }
                catch (InvalidOperationException)
                {
                    throw new SerializationException("GrammarDeserializationFailed");
                }

                reader.Close();

                grammarInput = new GrammarInput
                {
                    ActionDomainInput = actionDomainInput,
                    QuerySetInput = queryInput
                };
            }

            return grammarInput;
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
                FileName = $"scenario-{isoDateTimeString}Z",
                InitialDirectory = _defaultPath,
                RestoreDirectory = true,
                Filter = "TXT|*.txt",
                DefaultExt = "txt",
                Title = "Save Grammar Input File"
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
                Filter = "TXT|*.txt|XML|*.xml",
                DefaultExt = "txt",
                Title = "Open Grammar Input File"
            };

            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }
    }
}