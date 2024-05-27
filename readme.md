# Vim Renaissance

## Description

An experimental project to learn programming by creating a vim-like text editor from scratch.  
This project does not aim to be a full-fledged text editor, but rather a learning experience to understand the inner workings of a text editor.  
Meanwhile, it aims to be as simple as possible, so that it can be easily understood and modified.

## Personal Flavor and Motivation

Choosing C# as the programming language, was because it is the most familiar programming language for me at the time, and I was too lazy to learn C from the VIM source code.  
I also use this project to explore the performance improvement features of C# and .NET in the recent updates.  
Lastly, I would like to implement some features that are not available in VIM, and also building such a project is a good way to re-imagine how VIM should be like in the future. (I'm a advactor of VIM, and I want extend its spirit)

## Features (In Progress)

- [x] Rendor dummy text
- [x] Move cursor around the file
- [x] Move cursor by small word and big word
- [x] Choose QWERTY, Dvorak, or custom command mapping
- [ ] Resolve newlines in the buffer and terminal
- [ ] Save and load file
- [ ] Resize window and re-render text
- [ ] Basic text editing

## Installation

### Prerequisites

- .NET 8.0

## Usage

To run the project, execute `dotnet run` in the project directory.  

### IMPORTANT

Since the editor haven't fully implemented buffer and rendering, the editor load text from `assets/template.txt` and render it to the console.  
For now, the editor only support moving the cursor around the text, and editor cannot resolve newlines, so all the newlines are replaced with `|` character.  
In order to work with the editor properly, you need to resize the console window to fit the text so that every `|` character is in the last column of the console window.  
In short, it is recommended to run the editor in a separate terminal window, and resize the window to fit the text.