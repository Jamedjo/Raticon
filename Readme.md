# Decorate your Movie Collection

### Album Art for your movies!
Transforms your movie folders to look like movie posters with imdb ratings.

![Turns folders into Posters](https://cloud.githubusercontent.com/assets/1028741/3223815/a88088fa-f028-11e3-93b0-14ee8e821542.png)

## Overview

Quickly find your movie trailers, notes, etc. and view imdb ratings from within Windows Explorer.

* Raticon will help you the right rating and cover image for each movie in your collection.
* You can process your whole collection at once, or you can have Raticon automatically create icons when you add new movies.
* The app will process your collection using the **name of each sub-folder** to lookup the imdb id. This means any files relating to a movie must be in a folder named after the movie.


[ ![Download](https://cloud.githubusercontent.com/assets/1028741/4921172/38cadd08-6508-11e4-947d-c95c6a97e5e6.png) ](https://github.com/Jamedjo/Raticon/releases/latest)

## Instructions

### Add Folder

![Raticon App](https://cloud.githubusercontent.com/assets/1028741/4921034/28f90770-6507-11e4-95d5-eacf8739e808.png)

* Click "Select Folder" and select your movie folder.
* When all films have loaded click "Make Icons".
* You can 'Watch' a folder to automatically create icons when a new film is added.
* You may need to clear your thumbnail cache and restart explorer for the icons to update.

### Film Search

* If the film has an ambiguous title you will be asked to pick the correct film.
* If the sub-folder already contains a `_imdb_.nfo` file it will use the id from that file.

![Result Picker](https://cloud.githubusercontent.com/assets/1028741/3213279/4801a01e-ef87-11e3-83c9-cb14500f8b53.png)

### Cache Location

* If the wrong film has been selected you can remove the `_imdb_.nfo` file in the film's folder. You will also need to delete the `folder.ico` and `folder.jpg` images if you want them to be updated.
* Raticon caches film information in the `ProgramData` folder. E.g. `C:\ProgramData\Raticon\Cache`. If a rating is wrong/out-of-date you can force an update by deleting both the icon and the cache.