# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [5.4.0] - 2019-06-13
### Added
 - Added a calendar password parsing for associated call-in passwords

## [5.3.0] - 2019-06-07
### Changed
 - Parsing Asure Calendar notes for call-in information
 - Asure Calendar now refreshes when the port is assigned

## [5.2.0] - 2019-05-15
### Added
 - IBookingNumber implementations override ToString

### Changed
 - Fixed MockCalendar bookings

## [5.1.0] - 2019-01-10
### Added
 - Added Asure calendar control

## [5.0.2] - 2018-11-20
### Changed
 - Fixed null reference where Robin Calendar would update bookings with a null port

## [5.0.1] - 2018-10-30
### Changed
 - Fixed bug where Robin events would periodically vanish
 - Fixed bug where mock calendar control did not refresh
 - Fixed loading issue where devices would not fail gracefully when a port was not available

## [5.0.0] - 2018-10-18
### Added
 - Added Robin Calender device
 - Added booking number parsing
 - Added number parsing to Robin Device
 - Added support for multiple numbers

## [4.0.0] - 2018-04-24
### Changed
 - Removed suffix from assembly names
 - Using API event args
