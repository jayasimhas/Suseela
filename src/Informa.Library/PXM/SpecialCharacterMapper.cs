using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.Print_Studio_Templates.InDesign_connector.Special_Characters;
using Jabberwocky.Autofac.Attributes;
using Informa.Library.Utilities.CMSHelpers;

namespace Informa.Library.PXM
{
    public interface ISpecialCharacterMapper
    {
        Dictionary<char, char> WordToInDesignMap { get; }
        void ParseSpecialCharacters(string text, XElement baseFormattingElement, ref XElement parentElement);
    }

    [AutowireService]
    public class SpecialCharacterMapper : ISpecialCharacterMapper
    {
        private readonly IDependencies _dependencies;
        private readonly Guid _specialCharactersFolderId = new Guid(ItemIdResolver.GetItemIdByKey("SpecialCharacterFolder"));

        [AutowireService(true)]
        public interface IDependencies
        {
            ISitecoreService SitecoreService { get; set; }
        }

        public SpecialCharacterMapper(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public void ParseSpecialCharacters(string text, XElement baseFormattingElement, ref XElement parentElement)
        {
            var minSpecialCharacter = WordToInDesignMap.Keys.Min();
            var chars = text.ToCharArray();
            var processedCharacters = new StringBuilder();

            foreach (var character in chars)
            {
                ParseCharacter(character, minSpecialCharacter, baseFormattingElement, ref processedCharacters, ref parentElement);
            }

            if (processedCharacters.Length > 0)
            {
                AppendStringAsElement(processedCharacters.ToString(), baseFormattingElement, ref parentElement);
            }
        }

        private void ParseCharacter(char character, int minSpecialCharacter, XElement baseFormattingElement, ref StringBuilder processedCharacters,
            ref XElement parentElement)
        {
            if (character >= minSpecialCharacter && WordToInDesignMap.ContainsKey(character))
            {
                AppendStringAsElement(processedCharacters.ToString(), baseFormattingElement, ref parentElement);
                processedCharacters.Clear();

                var charSpan = baseFormattingElement != null
                    ? new XElement(baseFormattingElement)
                    : new XElement("Format");
                charSpan.SetAttributeValue(XName.Get("Font"), "Symbol");
                charSpan.Add(new XCData(new string(new[] {WordToInDesignMap[character]})));
                parentElement.Add(charSpan);
            }
            else
            {
                processedCharacters.Append(character);
            }
        }

        private static void AppendStringAsElement(string characters, XElement baseFormattingElement, ref XElement parentElement)
        {
            if (baseFormattingElement != null)
            {
                var format = new XElement(baseFormattingElement);
                format.Add(new XCData(characters));
                parentElement.Add(format);
            }
            else
            {
                parentElement.Add(new XCData(characters));
            }
        }

        private Dictionary<char, char> CreateWordToInDesignCharacterMap()
        {
            var mappings =
                _dependencies.SitecoreService.GetItem<ISpecial_Characters_Folder>(_specialCharactersFolderId)
                    ._ChildrenWithInferType
                    .OfType<ICharacter_Mapping>();

            var map = new Dictionary<char, char>();
            foreach (var characterMapping in mappings)
            {
                var from = HexToChar(characterMapping.From_Code);
                var to = HexToChar(characterMapping.To_Code);
                map[from] = to;
            }

            return map;
        }

        private char HexToChar(string hexString) => (char) int.Parse(hexString, NumberStyles.HexNumber);

        private Dictionary<char, char> _wordToIndesignMap;
        public Dictionary<char, char> WordToInDesignMap
            => _wordToIndesignMap ?? (_wordToIndesignMap = CreateWordToInDesignCharacterMap());

    }
}
