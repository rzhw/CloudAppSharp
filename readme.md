CloudAppSharp 0.9.2
===================

http://github.com/a2h/CloudAppSharp

Introduction
------------

It's a wrapper around the CloudApp API, in C# (not C Flat). This bad pun brought to you courtesy of a2h.

Instructions
------------

**TODO**

Changelog
---------

0.9.1 to 0.9.2:

* Lowercase emails before making a request; apparently CloudApp stores
  emails lowercased in their database...
* Added support for logging into CloudApp via HA1 (thanks ShaRose!)

0.9.0 to 0.9.1:

* Made CloudApp.UploadAsync() actually fully async

0.8.1 to 0.9.0:

* Added support for deleting files

0.8 to 0.8.1:

* Fixed concurrent I/O exceptions