# Change Log

## 1.3.0

- **New Feature:** added new editor `UIElement`, [`RangeSlider`](https://github.com/OmiyaGames/omiya-games-common/blob/main/Editor/VisualElements/RangeSlider.cs) -- a slider where one can set the minimum and maximum `float` value.  More importantly, unlike the built-in `MinMaxSlider`, the `RangeSlider` provides text fields to display and provide finer adjustments to the min and max values.
- **New Feature:** added new helper method, [`Helpers.CloneComponent<T>(Component original, GameObject destination)`](https://github.com/OmiyaGames/omiya-games-common/blob/main/Runtime/Helpers.cs) -- a method that adds a `Component` to a desginated `GameObject`, and copies over the fields from `original`.

## 1.2.3

- **Bug Fix:** Fixed editors of [`SerializableHashSet`](https://github.com/OmiyaGames/omiya-games-common/blob/main/Runtime/Serializables/SerializableHashSet.cs) and [`SerializableListSet`](https://github.com/OmiyaGames/omiya-games-common/blob/main/Runtime/Serializables/SerializableListSet.cs) to allow `null`/none elements, and stop throwing errors.

## 1.2.2

- **Refactor:** using `nameof()` for any argument-related exceptions.

## 1.2.1

- **Bug Fix:** slightly fixing how a random element is grabbed from [`RandomList`](https://github.com/OmiyaGames/omiya-games-common/blob/main/Runtime/RandomList.cs): if `CurrentElement` or `NextElement` is called while list is empty or only has one element, the reshuffle flag is now set in case the coder adds new elements to the list.

## 1.2.0

- **New Feature:** added new class, [`SerializableHashSet`](https://github.com/OmiyaGames/omiya-games-common/blob/main/Runtime/Serializables/SerializableHashSet.cs) -- a Set that can be adjusted in the inspector.
- **New Feature:** added new class, [`SerializableListSet`](https://github.com/OmiyaGames/omiya-games-common/blob/main/Runtime/Serializables/SerializableListSet.cs) -- a sorted Set that can be adjusted in the inspector.
- **New Enhancement:** allowing edits to [`Trackables`](https://github.com/OmiyaGames/omiya-games-common/blob/main/Runtime/Trackable/Trackable.cs) from the inspector trigger events.
- **New Enhancement:** improving inspector interface for [`RandomList`](https://github.com/OmiyaGames/omiya-games-common/blob/main/Runtime/RandomList.cs).

## 1.1.0

- **New Feature:** added new class, [`UndoHistory`](https://github.com/OmiyaGames/omiya-games-common/blob/main/Runtime/UndoHistory.cs)
- **New Enhancement:** added new helper function, [`Helpers.Destroy(Object)`](https://github.com/OmiyaGames/omiya-games-common/blob/main/Runtime/Helpers.cs), which actually was copy-pasted from Unity's Core RP Library package.

## 1.0.0

- Bumping up the version number to stable, given this library has been battle tested for long enough.

## 0.2.0-preview.2

- Enhancment: upgrading the assembly definitions and package files.

## 0.2.0-preview.1

- **New Feature:** added new VisualElements, [`Spacer.cs`](https://github.com/OmiyaGames/omiya-games-common/blob/main/Editor/VisualElements/Spacer.cs) and [`ProjectSettingsHeader.cs`](https://github.com/OmiyaGames/omiya-games-common/blob/main/Editor/VisualElements/ProjectSettingsHeader.cs).

## 0.1.4-preview.1

- **New Enhancement:** added XML documentation comments on all publicly accessible info in every script.
- **New Enhancement:** added method [`Helpers.RemoveDiacritics(string, StringBuilder)`](https://github.com/OmiyaGames/omiya-games-common/blob/main/Runtime/Helpers.cs) to remove invalid characters from a file name.
- **Removed Enhancement:** function `Helpers.ShortenUrl(string)` is moved to [Web package](https://openupm.com/packages/com.omiyagames.web/).
- **Documentation #11:** adding suggestions on packages that's more useful that uses this one as dependencies.

## 0.1.3-preview.1

- **New Feature #6:** added unit tests for [`BidirectionalDictionary.cs`](https://github.com/OmiyaGames/omiya-games-common/blob/main/Runtime/BidirectionalDictionary.cs), [`TestBidirectionalDictionary.cs`](https://github.com/OmiyaGames/omiya-games-common/blob/main/Tests/Editor/TestBidirectionalDictionary.cs)
- **New Enhancement:** Integrated DocFX auto-generated documentation, in favor of Doxygen.
- **New Enhancement:** Integrated Github Action for mirroring.

## 0.1.2-preview.1

- **New Feature #5:** added unit tests for [`RandomList.cs`](https://github.com/OmiyaGames/omiya-games-common/blob/main/Runtime/RandomList.cs), [`TestRandomList.cs`](https://github.com/OmiyaGames/omiya-games-common/blob/main/Tests/Editor/TestRandomList.cs)
- **New Enhancement #2:** for performance, [`RandomList.cs`](https://github.com/OmiyaGames/omiya-games-common/blob/main/Runtime/RandomList.cs) now stores a list of element-frequency pairs, rather than only the element themselves. Each element has to be unique; the paired frequency indicates how many times that element will appear in one cycle of iteration.

## 0.1.1-preview.1

- **Typo Fix:** fixing typo, "verison" to "version," on all source code documentation.
- **Documentation:** changed the format of the Doxygen-generated HTML file to have a sidebar.

## 0.1.0-preview.2

- **Documentation:** added author information to [package.json](https://github.com/OmiyaGames/omiya-games-common/blob/main/package.json).
- **Documentation:** added supported Unity release in [package.json](https://github.com/OmiyaGames/omiya-games-common/blob/main/package.json).

## 0.1.0-preview.1

- Initial commit, split off from the [Template Unity Project](https://github.com/OmiyaGames/template-unity-project).
