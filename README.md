# FrichText
A custom text format for presentation-ready formatted text that is terse and extremely fast to parse.

A FrichText string looks like the following (new-lines have been added clarity, but the format does not require new-lines to distinguish paragraphs).

>>>font 1: 'somefont'
>>>font 2: 'another font'
>>>font 3: 'yet another font'
>>>fs: 32
>>>lh: 48
This is some text. [b u]This text is bold and underlined. [k110]This text is bold and underlined with adjusted kerning.[/][/]

## Why?

Because the RichTextFormat kind of sucks, and I didn't know of any other simple formats that work for straight-forward literal expression of formatting.

I am using this library in a custom GUI project that needed an easy way to express and render formatted text.
