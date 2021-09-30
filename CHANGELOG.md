# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Changed
 - Fixed booking equality comparers to correctly compare dial contexts

## [9.4.1] - 2021-08-17
### Changed
 - CalendarManager - set max time before refreshing current booking to prevent timer max overflow

## [9.4.0] - 2021-05-14
### Added
 - Added event to CalendarManager for when the current booking changes.
 - Expose AsureDevice configured URI info as a new console node.
 
### Changed
 - ICalendarPoint implements IPoint<T>

## [9.3.1] - 2021-09-01
### Changed
 - Fixed issue with Calendar Manager not firing update when conference time didn't change

## [9.3.0] - 2021-01-25
### Added
 - Added optional property for call type to RegexCalendarParser & CalendarParsing.xml

## [9.2.0] - 2021-01-14
### Added
 - Added calendar features enum to track what each ICalendarControl supports (Edit booking, check-in/-out, create booking, etc...)
 - Added booking creation and editing features to ICalendarControl
 - Added CalendarManager which tracks a collection of ICalendarControls
 - Implemented booking creation and editing for Asure
 - Implemented booking creation and editing for Robin
 - Implemented method to get the timespan until the next meeting.
 - Implemented booking creation and editing for Google
 - Implemented booking creation and editing for Exchange
 - Implemented booking creation and editing for Office365

### Changed
 - Fixed Asure implementation to use UTC conversions properly
 - Asure settings no longer have Username & Password. The URI or Proxy Username & Password is now used for requests.

## [9.1.0] - 2020-07-14
### Changed
 - Calendar controls return the bookings for the full day
 - Fixed a bug in calendar parsing where numbers would sometimes match against multiple protocols

## [9.0.0] - 2020-06-18
### Added
 - PrintBookings console command will also print dial context info
 - Added CalendarPoint for tracking calendar features in a room

### Changed
 - MockCalendarDevice now inherits from AbstractMockDevice
 - Using new logging context
 - Mock bookings use UTC

## [8.0.1] - 2020-08-03
### Changed
 - Printed bookings in the CalendarControlConsole are now localized

## [8.0.0] - 2020-03-20
### Changed
 - Fixed web requests to use new web port response.
 - Fixed MockCalendarControl check-in/-out
 - Calendar dates are tracked in UTC

## [7.5.1] - 2020-08-10
### Changed
 - Removed proxy configuraton from MS Exchange calendar to resolve sandbox violations

## [7.5.0] - 2019-11-18
### Added
 - Calendar Check In/Check Out support
 - Added CheckedOutBy and CheckedOut properties to Asure ReservationData

## [7.4.0] - 2019-10-07
### Changed
 - Added an Organization ID Attribute to Robin Service Device settings that is now a part of the UserID request

## [7.3.0] - 2019-09-16
### Changed
 - Fixed a bug with Google calendar timezones
 - Office365 creation and modification dates are deserialized to DateTime
 - Google private key is sanitized to better support keys copied directly from google
 - Office365 is configured with a user email address instead of a user ID

## [7.2.0] - 2019-08-13
### Added 
 - Added iCalendar Calendar device
 - Added Google Calendar device.
 - Added Exchange Calendar device.
 
### Changed
 - Fixed Robin not returning events if the user endpoint fails.

## [7.1.0] - 2019-07-03
### Added 
 - Added Office365 calendar device
 - Added default "CalendarParsing.xml" path value to calendar settings
 
### Changed
 - De-duplicating booking info parsed from calendar event bodies

## [7.0.2] - 2019-05-02
### Changed
 - Using JsonConverters to deserialize Robin calendar JSON due to release obfuscation issues

## [7.0.1] - 2019-01-29
### Changed
 - Fixed issue with calendar booking de-duplication that was preventing bookings from updating

## [7.0.0] - 2019-01-14
### Changed
 - Dialing features refactored to fit new conferencing interfaces

## [6.0.0] - 2019-01-10
 - Added port configuration features to calendar devices

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
