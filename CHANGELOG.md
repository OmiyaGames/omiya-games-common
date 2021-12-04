# Change Log

## 1.1.0

- **New Feature:** added new class, [`UndoHistory`](https://github.com/OmiyaGames/omiya-games-common/blob/master/Runtime/UndoHistory.cs)
- **New Enhancement:** added new helper function, [`Helpers.Destroy(Object)`](https://github.com/OmiyaGames/omiya-games-common/blob/master/Runtime/Helpers.cs), which actually was copy-pasted from Unity's Core RP Library package.

## 1.0.0

- Bumping up the version number to stable, given this library has been battle tested for long enough.

## 0.2.0-preview.2

- Enhancment: upgrading the assembly definitions and package files.

## 0.2.0-preview.1

- **New Feature:** added new VisualElements, [`Spacer.cs`](https://github.com/OmiyaGames/omiya-games-common/blob/master/Editor/VisualElements/Spacer.cs) and [`ProjectSettingsHeader.cs`](https://github.com/OmiyaGames/omiya-games-common/blob/master/Editor/VisualElements/ProjectSettingsHeader.cs).

## 0.1.4-preview.1

- **New Enhancement:** added XML documentation comments on all publicly accessible info in every script.
- **New Enhancement:** added method [`Helpers.RemoveDiacritics(string, StringBuilder)`](https://github.com/OmiyaGames/omiya-games-common/blob/master/Runtime/Helpers.cs) to remove invalid characters from a file name.
- **Removed Enhancement:** function `Helpers.ShortenUrl(string)` is moved to [Web package](https://openupm.com/packages/com.omiyagames.web/).
- **Documentation #11:** adding suggestions on packages that's more useful that uses this one as dependencies.

## 0.1.3-preview.1

- **New Feature #6:** added unit tests for [`BidirectionalDictionary.cs`](https://github.com/OmiyaGames/omiya-games-common/blob/master/Runtime/BidirectionalDictionary.cs), [`TestBidirectionalDictionary.cs`](https://github.com/OmiyaGames/omiya-games-common/blob/master/Tests/Editor/TestBidirectionalDictionary.cs)
- **New Enhancement:** Integrated DocFX auto-generated documentation, in favor of Doxygen.
- **New Enhancement:** Integrated Github Action for mirroring.

## 0.1.2-preview.1

- **New Feature #5:** added unit tests for [`RandomList.cs`](https://github.com/OmiyaGames/omiya-games-common/blob/master/Runtime/RandomList.cs), [`TestRandomList.cs`](https://github.com/OmiyaGames/omiya-games-common/blob/master/Tests/Editor/TestRandomList.cs)
- **New Enhancement #2:** for performance, [`RandomList.cs`](https://github.com/OmiyaGames/omiya-games-common/blob/master/Runtime/RandomList.cs) now stores a list of element-frequency pairs, rather than only the element themselves. Each element has to be unique; the paired frequency indicates how many times that element will appear in one cycle of iteration.

## 0.1.1-preview.1

- **Typo Fix:** fixing typo, "verison" to "version," on all source code documentation.
- **Documentation:** changed the format of the Doxygen-generated HTML file to have a sidebar.

## 0.1.0-preview.2

- **Documentation:** added author information to [package.json](https://github.com/OmiyaGames/omiya-games-common/blob/master/package.json).
- **Documentation:** added supported Unity release in [package.json](https://github.com/OmiyaGames/omiya-games-common/blob/master/package.json).

## 0.1.0-preview.1

- Initial commit, split off from the [Template Unity Project](https://github.com/OmiyaGames/template-unity-project).
