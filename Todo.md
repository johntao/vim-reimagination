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

## Word Motion Algorithm
version 2 use symmetrical algorithm and reduced some of the ad-hoc algorithm
- `^` is the cursor position
- `|` is the boundary of the line 
  - p.s. so that "this text document itself" would not be affected by editor indentation
- `!` means exclusive search
- `>` means forward search
- `<` means backward search
- `w` means word
- `nw` means not word
  ### Subject1: Search WordBegin, Forward
  Use IsWordEndOrSpace to distinct patterns
  ```txt
  |case 1: word begin
  |qq  ww  eeeee
  |^
  |>nw>w!
  |
  |case 2: word end
  |qq  ww  eeeee
  | ^
  |>w!
  |
  |case 3: space begin
  |qq  ww  eeeee
  |  ^
  |>w!
  |
  |case 4: space end
  |qq  ww  eeeee
  |   ^
  |>w!
  |
  |case 1: word begin
  |qq  ww  eeeee
  |    ^
  |>nw>w!
  ```
  ### Subject2: Search WordEnd, Forward
  Use IsWordEndOrSpace to distinct patterns
  ```txt
  |case 1: word end
  |qq  ww  eeeee
  | ^
  |>w!>nw
  |
  |case 2: space begin
  |qq  ww  eeeee
  |  ^
  |>w!>nw
  |
  |case 3: space end
  |qq  ww  eeeee
  |   ^
  |>w!>nw
  |
  |case 4: word begin
  |qq  ww  eeeee
  |    ^
  |>nw
  |
  |case 1: word end
  |qq  ww  eeeee
  |     ^
  |>w!>nw
  ```
  ### Subject3: Search WordBegin, Backward
  Use IsWordBeginOrSpace to distinct patterns
  ```txt
  |case 1: word begin
  |qqqqq  ww  ee
  |           ^
  |<w!<nw
  |
  |case 2: space end
  |qqqqq  ww  ee
  |          ^
  |<w!<nw
  |
  |case 3: space begin
  |qqqqq  ww  ee
  |         ^
  |<w!<nw
  |
  |case 4: word end
  |qqqqq  ww  ee
  |        ^
  |<nw
  |
  |case 1: word begin
  |qqqqq  ww  ee
  |       ^
  |<w!<nw
  ```
  ### Subject4: Search WordEnd, Backward
  Use IsWordBeginOrSpace to distinct patterns
  ```txt
  |case 1: word end
  |qqqqq  ww  ee
  |            ^
  |<nw<w!
  |
  |case 2: word begin
  |qqqqq  ww  ee
  |           ^
  |<w!
  |
  |case 3: space end
  |qqqqq  ww  ee
  |          ^
  |<w!
  |
  |case 4: space begin
  |qqqqq  ww  ee
  |         ^
  |<w!
  |
  |case 1: word end
  |qqqqq  ww  ee
  |        ^
  |<nw<w!
  ```

## OO abstraction

- Moral of the day...
- Editor knows about the renderer which is Console for now
  - also knows about the buffer which is 1D `ReadOnly<Span>` for now
  - editor also instantiate the motion algorithm
- Motion algorithm accept renderer and buffer as input
  - motion calculates the new cursor position and return it
- We make this design by the nature limitation of readonly ref struct
  - it does not simply support inheritance or static readonly member
  - we think twice before making a class
  - in the end the Motion algorithm is a class and it cannot hold buffer as a static readonly member
  - so the final design pass in buffer as a parameter