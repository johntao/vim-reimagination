- [x] add text in the background. text aware cursor motion
- Motion
  - resolve word-based motions
    - [x] e, r first implementation
    - [x] e needs to be fixed
      - prefer symmetrical algorithm over ad-hoc algorithm
    - [ ] make status bar more informative
      - text-object algorithm
    - [ ] q, w first implementation
- Base framework
  - [ ] get current line number
  - [ ] remove text in the background
  - [ ] resolve newlines
  - [ ] resolve different buffer types
  - [ ] yank, putReplace
  - [ ] editable: replace, delete, change
  - [ ] set up boundaries for status bar, motion and rendering
- Advanced framework
  - [ ] should be able to scroll
  - [ ] editable: insert
  - [ ] putInsert
  - [ ] undo, redo, search, save, quit
  - [ ] IHasRegister
  - [ ] Multiple cursors
  - [ ] IHasHistory
  - [ ] visual mode
- Advanced Hotkey Trigger or Accessibility
  - [ ] resolve multiple key press
  - [ ] resolve multiple key inputs
  - [ ] flash visual
- Vision
  - remove ex command while keeping all the capacity
  - support any kinds of keyboard layout
  - should be able to run on any platform
    - hence, the API should be platform agnostic
    - beaware if platform doesn't support Console.Left, Console.Top
    - should at least migrate to Terminal.Gui
    - afterward, we might have better support for migration to other platforms
    - probably, the best way to do this is to develop with lower level API

## word algorithm rework

### version 1
  ```txt
  |Search WordBegin Forward 4,2
  |qqq  www  eee
  |^
  |>nw>w
  |qqq  www  eee
  | ^
  |>nw>w
  |qqq  www  eee
  |  ^
  |>nw>w
  |qqq  www  eee
  |   ^
  |>w
  |qqq  www  eee
  |    ^
  |>w
  |qqq  www  eee
  |     ^
  |>nw>w
  |Search WordEnd Forward 4(2),2
  |qqq  www  eee
  |  ^
  |>w!>nw
  |qqq  www  eee
  |   ^
  |>w>nw
  |qqq  www  eee
  |    ^
  |>w>nw
  |qqq  www  eee
  |     ^
  |>nw
  |qqq  www  eee
  |      ^
  |>nw
  |qqq  www  eee
  |       ^
  |>w!>nw
  |Search WordBegin Backward 4(2),2
  |qqq  www  eee
  |          ^
  |<w!<nw
  |qqq  www  eee
  |         ^
  |<w<nw
  |qqq  www  eee
  |        ^
  |<w<nw
  |qqq  www  eee
  |       ^
  |<nw
  |qqq  www  eee
  |      ^
  |<nw
  |qqq  www  eee
  |     ^
  |<w!<nw
  |Search WordEnd Backward 4,2
  |qqq  www  eee
  |            ^
  |<nw<w
  |qqq  www  eee
  |           ^
  |<nw<w
  |qqq  www  eee
  |          ^
  |<nw<w
  |qqq  www  eee
  |         ^
  |<w
  |qqq  www  eee
  |        ^
  |<w
  |qqq  www  eee
  |       ^
  |<nw<w
  ```
### version 2 symmetrical and reduced
  ```txt
  |Search WordBegin Forward 3,3 IsWordEndOrSpace
  |qqq  www  eee
  |^
  |>nw>w!
  |qqq  www  eee
  | ^
  |>nw>w!
  |qqq  www  eee
  |  ^
  |>w!
  |qqq  www  eee
  |   ^
  |>w!
  |qqq  www  eee
  |    ^
  |>w!
  |qqq  www  eee
  |     ^
  |>nw>w!
  |Search WordEnd Forward 4,2 IsWordEndOrSpace
  |qqq  www  eee
  |  ^
  |>w!>nw
  |qqq  www  eee
  |   ^
  |>w!>nw
  |qqq  www  eee
  |    ^
  |>w!>nw
  |qqq  www  eee
  |     ^
  |>nw
  |qqq  www  eee
  |      ^
  |>nw
  |qqq  www  eee
  |       ^
  |>w!>nw
  |Search WordBegin Backward 4,2
  |qqq  www  eee
  |          ^
  |<w!<nw
  |qqq  www  eee
  |         ^
  |<w!<nw
  |qqq  www  eee
  |        ^
  |<w!<nw
  |qqq  www  eee
  |       ^
  |<nw
  |qqq  www  eee
  |      ^
  |<nw
  |qqq  www  eee
  |     ^
  |<w!<nw
  |Search WordEnd Backward 3,3
  |qqq  www  eee
  |            ^
  |<nw<w!
  |qqq  www  eee
  |           ^
  |<nw<w!
  |qqq  www  eee
  |          ^
  |<w!
  |qqq  www  eee
  |         ^
  |<w!
  |qqq  www  eee
  |        ^
  |<w!
  |qqq  www  eee
  |       ^
  |<nw<w!
  ```