## Word Motion Algorithm v2 and v3

  no time to document the produce or design of the algorithm

## Word Motion Algorithm v1
  use symmetrical algorithm and reduced some of the ad-hoc algorithm
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

## aggressive oo design

  - mark everything sealed if it's not meant to be inherited
  - hide every POCO inside a class if it's not used elsewhere

## ReadOnlySpan

  - pros
    - whenever we slice the buffer, we don't need to allocate new memory
    - that's all
  - cons
    - now we need to pass the buffer around. there's no way to store it as a static readonly member
    - we could still store the buffer in a ref struct
    - now we have to declare ref struct everywhere

## About Markdown table rendering

  - favor jagged array over 2D array, since we encapsulate data into a class, jagged array is more suitable in this case
  - we took some time debugging null reference exception, I thought it was caused by Func<> or Action<> closure, but it was actually caused by the default(string) == null
  - I'm still wondering what's the best way to render a table with 2D array
  - we found out that it is possible to reduce for loop init from 3 segments to 2 segments (use prefix increment and -1 init value)
  - there's GUI button for adding new config in launch.json; however, there is no GUI button for adding new tasks in tasks.json (command palette is the only way to add new tasks)
  - new syntax in the algorithm: use null-conditional operator in a comparison; null-coalescing assignment
  - I don't think there's a way to fetch the first element of a sequence and then reset the enumerator to the beginning... or at least the code would be ugly
  - don't know why but the integrated terminal cannot take Console.Clear()
  - we left the initialization of separator of the table header at call site, since we don't want to use `List<string[]>` and then insert `string[]` at index 1
    - however, it turns out that we can omit the initialization and keep the code clean in the same time
    - since, the only thing we need to render the separator is the length of the columns, which is very accessible in the algorithm
  - we found out that the default char is a control character, which will consume a space in the memory, but vanish after rendering
  - procedural programming build on top of statements and function calling footprints
    - think of how sequential coupling were made, think of how build up coding blocks with more statements, how each of them interact with each other
    - this are the basic of procedural programming
    - oo programming build on top of object and context and encapsulation
    - think of how context are stored or cached, think of how we can expose or encapsulate the object
      - keywords: scope, scope, scope!
    - one of the reason that we cannot set _width as readonly is that we use LINQ in the algorithm
    - which delegate the execution of those statements to some other place
    - which means the _width is no longer readonly (i.e. only scoped in fields and constructor)

## about cross platform

  - we should probably abstract the console renderer