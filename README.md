# [Omiya Games](https://www.omiyagames.com/) - Common

[![openupm](https://img.shields.io/npm/v/com.omiyagames.common?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.omiyagames.common/) [![Documentation](https://github.com/OmiyaGames/omiya-games-common/workflows/Host%20DocFX%20Documentation/badge.svg)](https://omiyagames.github.io/omiya-games-common/) [![Ko-fi Badge](https://img.shields.io/badge/donate-ko--fi-29abe0.svg?logo=ko-fi)](https://ko-fi.com/I3I51KS8F) [![License Badge](https://img.shields.io/github/license/OmiyaGames/omiya-games-common)](/LICENSE.md) 

Common scripts used within [Omiya Games](https://www.omiyagames.com/)' many tools and libraries.  Some useful tools that uses this package:

- [Multiplatform Build Settings](https://openupm.com/packages/com.omiyagames.builds/)
- [Web Security](https://openupm.com/packages/com.omiyagames.web.security/)
- [Cryptography](https://openupm.com/packages/com.omiyagames.cryptography/)

## Install

There are two common methods for installing this package.

### Through [Unity Package Manager](https://docs.unity3d.com/Manual/upm-ui-giturl.html)

Unity's own Package Manager supports [importing packages through a URL to a Git repo](https://docs.unity3d.com/Manual/upm-ui-giturl.html):

1. First, on this repository page, click the "Clone or download" button, and copy over this repository's HTTPS URL.  
2. Then click on the + button on the upper-left-hand corner of the Package Manager, select "Add package from git URL..." on the context menu, then paste this repo's URL!

While easy and straightforward, this method has a few major downside: it does not support dependency resolution and package upgrading when a new version is released.  To add support for that, the following method is recommended:

### Through [OpenUPM](https://openupm.com/)

Installing via [OpenUPM's command line tool](https://openupm.com/) is recommended because it supports dependency resolution, upgrading, and downgrading this package.  If you haven't already [installed OpenUPM](https://openupm.com/docs/getting-started.html#installing-openupm-cli), you can do so through Node.js's `npm` (obviously have Node.js installed in your system first):
```
npm install -g openupm-cli
```
Then, to install this package, just run the following command at the root of your Unity project:
```
openupm add com.omiyagames.common
```

## Resources

- [Documentation](https://omiyagames.github.io/omiya-games-common/)
- [Change Log](https://omiyagames.github.io/omiya-games-common/manual/changelog.html)

## LICENSE

Overall package is licensed under [MIT](/LICENSE.md), unless otherwise noted in the [3rd party licenses](/THIRD%20PARTY%20NOTICES.md) file and/or source code.

Copyright (c) 2014-2022 Omiya Games
