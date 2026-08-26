INCLUDE globals.ink
EXTERNAL inputName()
EXTERNAL continueTimeline()
EXTERNAL StartBattle(enemy)
EXTERNAL ContinueBattle(turn)
EXTERNAL OpenGate(level)
EXTERNAL AddItem(itemName, count)
->Main

===Main===
#speaker:Unknown
#layout:right
#portrait:MonbunSpeaker
!!!
~continueTimeline()
#speaker:???
#layout:left
#portrait:PlayerSpeaker
Wait
~continueTimeline()
#speaker:Unknown
#portrait:MonbunSpeaker
#layout:right
What are you doing here!?
#speaker:???
#layout:left
#portrait:PlayerSpeaker
I'm sorry but...
May i ask...
Where exactly is this place?
#speaker:Unknown
#layout:right
#portrait:MonbunSpeaker
...
#speaker:???
#layout:left
#portrait:PlayerSpeaker
And...
Who are you?
It seems like you know me
#speaker:Unknown
#layout:right
#portrait:MonbunSpeaker
?
...
So that's how it is
#speaker:???
#layout:left
#portrait:PlayerSpeaker
What do you mean?
#speaker:Unknown
#layout:right
#portrait:MonbunSpeaker
Oh nothing
I was just a little surprised
#speaker:???
#layout:left
#portrait:PlayerSpeaker
Why?
#speaker:Unknown
#layout:right
#portrait:MonbunSpeaker
Because it’s been a very long time
since anyone has come here
#speaker:???
#layout:left
#portrait:PlayerSpeaker
...
#speaker:Unknown
#layout:right
#portrait:MonbunSpeaker
if i may ask...
what brings you here?
#speaker:???
#layout:left
#portrait:PlayerSpeaker
...
I don't know
It's difficult for me to try to remember it
In fact...
I don't even remember my own name
#speaker:Unknown
#layout:right
#portrait:MonbunSpeaker
...
I feel sorry for you
In that case
You'll remember it little by little
#speaker:???
#layout:left
#portrait:PlayerSpeaker
Yeah...
Thanks, maybe?
#speaker:Unknown
#layout:right
#portrait:MonbunSpeaker
Let me introduce myself
#speaker:Monbun
My name is Monbun
Nice to meet you
#speaker:???
#layout:left
#portrait:PlayerSpeaker
I...
I don't know how to introduce myself
I don't even remember my name
#speaker:Monbun
#layout:right
#portrait:MonbunSpeaker
It's okay
For now, just use whatever name you like
#speaker:???
#layout:left
#portrait:PlayerSpeaker
Anything?
#speaker:Monbun
#layout:right
#portrait:MonbunSpeaker
Yes
Because a name is just a label
It’s not who you really are
#speaker:???
#layout:left
#portrait:PlayerSpeaker
...
...
In that case, my name is
~inputName()
...
#speaker:???
#layout:left
#portrait:PlayerSpeaker
{MCName}
#speaker:Monbun
#layout:right
#portrait:MonbunSpeaker
What a good name
Okay, it's nice to see you again {MCName}
...
{MCName}
You’ve really forgotten everything about yourself haven’t you?
#speaker:???
#layout:left
#portrait:PlayerSpeaker
Yes...
#speaker:Monbun
#layout:right
#portrait:MonbunSpeaker
Maybe this gate is the answer for you
#speaker:???
#layout:left
#portrait:PlayerSpeaker
The answer?
#speaker:Monbun
#layout:right
#portrait:MonbunSpeaker
Yes
this gate can reveal things about you
your past
and the reason why you're here
#speaker:???
#layout:left
#portrait:PlayerSpeaker
Is that true?
#speaker:Monbun
#layout:right
#portrait:MonbunSpeaker
Yes
But it’s very difficult to get that
You need certain things to find the answer
...
But don't worry
I can help you
#speaker:???
#layout:left
#portrait:PlayerSpeaker
Do you want to help me?
What do I need to do to help you?
#speaker:Monbun
#layout:right
#portrait:MonbunSpeaker
It's easy
Follow me for a moment
~continueTimeline()
... #layout:right #portrait:MonbunSpeaker
This is the gate
I can show you specific locations through this gate
And you need to collect eight items at each location you enter.
#speaker:???
#layout:left
#portrait:PlayerSpeaker
Eight items, what items?
#speaker:Monbun
#layout:right
#portrait:MonbunSpeaker
The items are the balls sealed within a certain area.
#speaker:???
#layout:left
#portrait:PlayerSpeaker
...
#speaker:Monbun
#layout:right
#portrait:MonbunSpeaker
Basically
There’s a unique area with a sealed ball in the middle
#speaker:???
#layout:left
#portrait:PlayerSpeaker
That's it?
#speaker:Monbun
#layout:right
#portrait:MonbunSpeaker
Oh
Of course not yet
each of those sealed balls is guarded by a shadow
I tried to defeat it but failed.
#speaker:???
#layout:left
#portrait:PlayerSpeaker
How can I beat him?
#speaker:Monbun
#layout:right
#portrait:MonbunSpeaker
Don't worry about it
I see the potential in you that can help you beat them
~PrologStart = true
I'll teach you a spell that can defeat those shadows
~continueTimeline()
~StartBattle(0)
~ContinueBattle(false)
... #layout:right #portrait:MonbunSpeaker
So you can do anything during your turn
You can use skills to attack, heal, and stun
You can also use counter to reduce the damage you take, and...
There is a chance you can reflect the damage back at the enemy
Here are also items you can use during battle
But the only item I’ve gotten so far on this journey is this heal
~AddItem("Potion", 5)
You Get 5 Potion #layout:hide
Though I’m sure it’ll come in handy for you later #layout:right #portrait:MonbunSpeaker
~ContinueBattle(true)
... #layout:right #portrait:MonbunSpeaker
Nice!!!
I knew you have the potential
#speaker:???
#layout:left
#portrait:PlayerSpeaker
Y-
Yeah
#speaker:Monbun
#layout:right
#portrait:MonbunSpeaker
I think you're ready for this
Are you ready to go?
* [Yes]
->Yes
* [Wait]
->No

===Yes===
Nice
I like your courage
Wait a minute, I'll open it for you
~OpenGate(1)
->END

===No===
It's okay, i can wait
->END


