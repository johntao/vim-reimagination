## buffer and window

  - in dotnet Console, buffer >= window

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

## Aggressive Object-Oriented Programming

- Mark everything as sealed if it's not meant to be inherited.
- Hide or nest every POCO inside a class or method if it's not used elsewhere.
  - POCOs only accessible to another type.
  - Interfaces that don't have multiple implementations.
  - Private methods that aren't called by other methods.
- Reduce the number of interface members if possible.
- Reduce the number of public method parameters if possible.

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

## about host service

  - we tried to use host service to run the editor, however, it seems not so intuitive
  - we MUST reuse the cancellation token, otherwise, the editor will not be able to close properly
  - we MUST wrap the code by additional Task.Run
  - we also need to return Task.CompletedTask in the StartAsync method, MUST NOT block the StartAsync method
  - we MUST add a small delay before the editor starts, otherwise, the editor will not be able to start properly
    - since, there's a default ConsoleLifetime output before the editor starts
  - we tried to find a way to simplify these steps, but it seems that we cannot do that
    - the default ApplicationLifetime and HostService does not provide any hook
    - BackgroundService is not suitable for this case, since the editor blocks the thread with Console.ReadKey()

## about class naming

  - [reference: class name convention](https://stackoverflow.com/questions/8614356/using-verbs-in-class-names)
  - we know that we should name the class with a noun
    - however, it is difficult to name the class with a noun when the application is procedural and small
    - we should probably name the class with a noun when the application grows bigger
    - something like service or a manager or a controller
  - for now, we just intentionally name the class with a verb...
    - I think it is fine, since the class is just simple Task and expose a single method `Run()`
    - after postfixing the class with Task, it is clear that the class is a simple task
    - this is a good way to name the class when the class is simple and procedural without violating the naming convention

## about the Console API

  - [Windows Console API](https://docs.microsoft.com/en-us/windows/console/console-functions)
  - we found out that dotnet only support very limited console API...
  - the full API is only available in Windows using clang
  - which contradicts to our initial goal to avoid using clang

## about implementing the buffer

  - we need to understand stackalloc before implementing the buffer
  - probably a few tests required to ensure the buffer is working properly
  - we could implement a version without using stackalloc first, then implement the stackalloc version
  - we should probably separate the two implementations into two different classes
  - best practice
    - use ref struct only in mutation heavy code
    - use ref struct only in the innermost layer of the code
    - store mutation result in non-ref struct (since the max size of stack is less than 2 MB)
    - make sure after the computation, the ref struct is no longer used (the stack is poped)
    - should initialize the buffer with window size first, then, do the mutation
  - the result
    - it's okay. but, we still have some space to improve
    - [x] replace operator overloading with instance method is quite ugly, MUST FIX IT
    - [x] we found out the buffer is just a char[]. we could probably simplify the algorithm a bit
      - it is so weird that we must use `AsSpan()` to make `CopyTo()` work
      - I wonder if there's a simpler way to do it
    - [x] we should probably shrink some interface whenever possible
    - [ ] the template is acting weird, if we omit the last space in the last line, the template will not be rendered properly