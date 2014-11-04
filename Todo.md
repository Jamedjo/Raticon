### Todo

* Should be able to cancel "Make Icons"
* Get `folder.jpg`s in background instead of during icon creation
* LookupChoice GUI should be different when no movie found / no internet. Should not error on pick selected.
* Ability to watch a folder for changes and setup icons when a film is added (don't get tangled with app creating the folder though).

### Ideas

* Store folders in settings
* Sort films by heading. Makes it easy to view top films
* Double-click a film opens it in explorer
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