# FrichText
A custom text format for presentation-ready formatted text that is terse and extremely fast to parse.

A FrichText string looks like the following (new-lines have been added clarity, but the format does not require new-lines to distinguish paragraphs).

&gt;&gt;&gt;font 1: 'somefont'

&gt;&gt;&gt;font 2: 'another font'

&gt;&gt;&gt;font 3: 'yet another font'

&gt;&gt;&gt;fs: 12

&gt;&gt;&gt;lh: 18

This is some text. [b u]This text is bold and underlined. [k110]This text is bold and underlined with adjusted kerning.[/][48 lh72]This text is also underlined and bold, but with unaltered kerning and a font-size 48 with line-height of 72.[/][/]

## Why?

Because the RichTextFormat kind of sucks, and I didn't know of any other simple formats that work for straight-forward literal expression of formatting.

I am using this library in a custom GUI project that needed an easy way to express and render formatted text.
