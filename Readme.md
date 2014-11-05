# Decorate your Movie Collection

### Album Art for your Movies!
Transform plain folders to into movie posters with Rating Icons.

![Turns folders into Posters](https://cloud.githubusercontent.com/assets/1028741/3223815/a88088fa-f028-11e3-93b0-14ee8e821542.png)

## Overview

Locate your movie trailers, notes, etc. at a glance. View IMDb ratings from the convenience of Windows Explorer.

Raticon will help you find the right rating and cover image for each movie in your collection.

You can process your whole collection at once, or you can have Raticon automatically create icons when you add new movies.

The app will process your collection using the **name of each sub-folder** to lookup the IMDb id. This means any files relating to a movie must be in a folder named after the movie.


[ ![Download](https://cloud.githubusercontent.com/assets/1028741/4921172/38cadd08-6508-11e4-947d-c95c6a97e5e6.png) ](https://github.com/Jamedjo/Raticon/releases/latest)

## Instructions

### Add Folder

![Raticon App](https://cloud.githubusercontent.com/assets/1028741/4921034/28f90770-6507-11e4-95d5-eacf8739e808.png)

* Click "Select Folder" and find your movie folder.
* When all films have loaded click "Make Icons".
* When you 'Watch' a folder Raticon will create icons as soon as you add a new film.
* You may need to clear your thumbnail cache and restart explorer for the icons to update.

### Film Search

* If the film has an ambiguous title you will be asked to pick the correct film.
* If the sub-folder already contains a `_imdb_.nfo` file it will use the id from that file.

![Result Picker](https://cloud.githubusercontent.com/assets/1028741/3213279/4801a01e-ef87-11e3-83c9-cb14500f8b53.png)

### Cache Locations

If the wrong film has been selected you can remove the `_imdb_.nfo` file in the film's folder. You will also need to delete `folder.ico` and `folder.jpg` so Raticon knows to update the images.

Raticon caches film information in the `ProgramData` folder. E.g. `C:\ProgramData\Raticon\Cache`. If a rating is wrong/out-of-date you can force an update by deleting both the icon and the cache.