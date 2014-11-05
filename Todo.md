### Todo

* Should be able to cancel "Make Icons"
* Get `folder.jpg`s in background instead of during icon creation

### Ideas

* Store folders in settings
* More than one folder at once
* Tickboxes and option to build the following
  * hasFolder.jpg
  * has.rating
  * has_imdb_.nfo
* Retry imdb id lookup with new search shouldn't go to end of queue
* Last ListView column should stretch on resize
* Progress complete dialog could have button to launch folder.
* Status bar to show how many ratings/imdbIds/folder.jpgs are being fetched.
* CLI `--icon-status` could list booleans for folderjpg, foldericon and readonly/ini setup.
* CLI `--undecorated` could list movies which are missing icons.
* CLI `--gui D:\Path` could set GUI folder.
* CLI `--decorate --norating` could make it process folder.jpg into .ico without any film lookup or ratings. Use for TV shows?
* Watcher taskbar notification could show recently processed films.
* Should not be able to pick both --watch and --gui until that is made possible. Or could make watch always use gui.
* FolderWatcher should wait for some delay after change. Scenarios: New Folder may be renamed. Folder without folder.jpg may get one from another service. Folder without imdb.nfo may get one from another service.
* Gui Result picker error layout could be cleaner.
* Could eliminate 'Make Icons' button and have them auto-process. Would need to find a place to mention clearing thumbnail cache.
* MediaCollection should refresh on FileSystem change.
* End2end testing could help prevent the introduction of threading bugs.
* Unicode? Hyphen in TV series year?
* Raticon can be initialized in background watch mode. Consider task bar icon.
