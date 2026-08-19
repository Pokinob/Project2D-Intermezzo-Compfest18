INCLUDE globals.ink
EXTERNAL inputName()
EXTERNAL continueTimeline()
->main

===main===
#speaker:Unknown
#layout:right
!!!
~continueTimeline()
...
#speaker:???
#layout:left
Wait
~continueTimeline()
...
#speaker:Unknown
#layout:right
What are you doing here!?
#speaker:???
#layout:left
I'm sorry but...
May i ask...
Where exactly is this place?
#speaker:Unknown
#layout:right
...
#speaker:???
#layout:left
And...
Who are you?
It seems like you know me
#speaker:Unknown
#layout:right
?
...
So that's how it is
#speaker:???
#layout:left
What do you mean?
#speaker:Unknown
#layout:right
Oh nothing
I was just a little surprised
#speaker:???
#layout:left
Why?
#speaker:Unknown
#layout:right
Because it’s been a very long time
since anyone has come here
#speaker:???
#layout:left
...
#speaker:Unknown
#layout:right
if i may ask...
what brings you here?
#speaker:???
#layout:left
...
I don't know
It's difficult for me to try to remember it
In fact...
I don't even remember my own name
#speaker:Unknown
#layout:right
...
I feel sorry for you
In that case
You'll remember it little by little
#speaker:???
#layout:left
Yeah...
Thanks, maybe?
#speaker:Unknown
#layout:right
Let me introduce myself
#speaker:Monbun
My name is Monbun
Nice to meet you
#speaker:???
#layout:left
I...
I don't know how to introduce myself
I don't even remember my name
#speaker:Monbun
#layout:right
It's okay
For now, just use whatever name you like
#speaker:???
#layout:left
Anything?
#speaker:Monbun
#layout:right
Yes
Because a name is just a label
It’s not who you really are
#speaker:???
#layout:left
...
...
In that case, my name is
~inputName()
...
#speaker:???
#layout:left
{MCName}
#speaker:Monbun
#layout:right
What a good name
Okay, it's nice to see you again {MCName}
...
{MCName}
You’ve really forgotten everything about yourself haven’t you?
#speaker:???
#layout:left
Yes...
#speaker:Monbun
#layout:right
Maybe this gate is the answer for you
#speaker:???
#layout:left
The answer?
#speaker:Monbun
#layout:right
Yes
this gate can reveal things about you
your past
and the reason why you're here
#speaker:???
#layout:left
Is that true?
#speaker:Monbun
#layout:right
Yes
But it’s very difficult to get that
You need certain things to find the answer
...
But don't worry
I can help you
#speaker:???
#layout:left
Do you want to help me?
What do I need to do to help you?
#speaker:Monbun
#layout:right
It's easy
Follow me for a moment
~continueTimeline()
...
This is the gate
I can show you specific locations through this gate
And you need to collect eight items at each location you enter.
#speaker:???
#layout:left
Eight items, what item?
#speaker:Monbun
#layout:right
The item is a ball sealed within a certain area.
#speaker:???
#layout:left
...
#speaker:Monbun
#layout:right
Basically
There’s a unique area with a sealed ball in the middle
#speaker:???
#layout:left
That's it?
#speaker:Monbun
#layout:right
Oh
Of course not yet
each of those sealed balls is guarded by a shadow
I tried to defeat it but failed.
#speaker:???
#layout:left
How can I beat him?
#speaker:Monbun
#layout:right
Don't worry about it
I see the potential in you that can help you beat them
~PrologStart = true
I'll teach you a spell that can defeat those shadows
~continueTimeline()
->END
