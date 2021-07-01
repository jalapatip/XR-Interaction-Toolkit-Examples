using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENUM_XROS_VES_Layer
{
    Debug,
    Sensory,
    Privacy,
    Management,
    Cosmetic,
    Avatar,
    Dev,
    Gameplay
}

/*
Paper: Virtual Equipment System: Expansion to Address Alternate Contexts
 
 
\section{Potential Contexts for VES}
There are many different contexts that one may want to use Virtual Equipment System for. We propose that the users have multiple Virtual Equipment Sets worn at the same time. Each Set is collection of Virtual Equipment and typically intended and geared toward a specific context. Some possible contexts are: Sensory, Privacy, Management, Cosmetic, Avatar, Dev, and App/Game Specific.

Of the many contexts, we believe the first three should be universal and supported on an Operating System (OS) level. By supporting them at the OS-level, it would allow users a familiar and universal interface to fall back on regardless of the VR world they are in.

In any given context, there may be multiple Sets associated with that context. For example, the user may have many different Game-Specific Sets for a fantasy game, such as a Warrior Set, a Thief Set, and a Wizard Set.

\subsection{Different Sets and their Contexts}
\textbf{Sensory Set for Incoming Sensory Data}
In any VR application, the user benefits from being able to quickly adjust sensory settings. This is one of the candidate for universal implementation as without sensory inputs to the user, there would be no VR. Users would use the Virtual Equipment located near their sensory organs to adjust and access its corresponding settings.

While not strictly incoming sensory data, interaction settings are also included here as they tend to be grouped together in traditional menus. This may include interacting with a Virtual Microphone for audio recording settings, Virtual Controller to adjust input settings, and Virtual Shoe for locomotion settings.

\textbf{Privacy Set for Outgoing User Data (Privacy)}
Users in virtual reality are increasingly becoming part of the computing environment, with data collected (with permission) that the user may not be aware of. For example, the activity and location of the user's motion controller provide clues to what the user is doing at the moment. Any data that the user may not want others to receive or utilize would fall under the jurisdiction of the privacy Set.

In our implementation of the privacy Virtual Equipment System, we utilize the voodoo doll metaphor in addition to the Virtual Equipment System to provide the user with additional access to privacy options that are not normally associated with a body part, such as GPS location, or difficult to get to, such as the user's heart. The combination techniques provide the user with a powerful and intuitive interface that would be highly useful if it is implemented at the OS-level.
\begin{figure}
 \centering % avoid the use of \begin{center}...\end{center} and use \centering instead (more compact)
 \includegraphics[width=0.5\columnwidth]{pictures/heart}
 \caption{VES combined with Voodoo Doll Metaphor for the privacy context. The user's left controller has grabbed the Heart organ, allowing the user to enable/disable tracking for pulse rate}
 \label{fig:heart}
\end{figure}

\textbf{Management Set for VES Management}
This Set utilizes a special instance of the VES where the user can manage settings related to Virtual Equipment Systems. Broadly, the user would be able to manage which Virtual Equipment Sets to use, swap out the equipment equipped within a Virtual Equipment Set, and fine tune the position of the equipment slots. 

Given that VES involves whole body interaction, it is ideal for VES to also be adjusted in the same manner. The user should be able to move Equipment slots using the same motion controller in the same way that he would access an equipment stored in that slot. Using a traditional 2D menu would increase the time needed to customize the interaction to the user's end result.

This is the third candidate for being a universal VES Set as its role becomes increasingly crucial once the user is dealing with more than one Equipment Set.

\textbf{Cosmetic Set for Outgoing User Data (Cosmetic)}
In an multi-user environment, the user may want to look a certain way to the other users. In many games featuring equipment, the user typically has equipment that looks a certain way and provides gameplay bonuses to the user. Some games also have the option of wearing cosmetic or vanity equipment that override the appearance of the functional equipment the user has equipped. Similarly, in VR, the user may have a Set for this context. The user would appear to others as wearing whichever equipment they have on in this Set. The user would also be able to interact with these equipment to access its related cosmetic settings. For example, the user may interact with the eye-patch worn at the eye-slot to change to a different eye equipment such as a monocle. 

\begin{figure}[hbt!]
 \centering % avoid the use of \begin{center}...\end{center} and use \centering instead (more compact)
 \includegraphics[width=0.5\columnwidth]{pictures/PlacingAA}
 \caption{In the Alternate Avatar Context, the user is placing an Alternate Avatar that the user can later see, hear, or speak through. The user can also possess the Alternate Avatar to become that avatar}
 \label{fig:PlacingAA}
\end{figure}
\textbf{Avatar Set for Using Alternate Avatars}
In this Set, the user has access to Equipment that allow the user to place Alternate Avatars and its lesser versions in the world as shown in \ref{fig:PlacingAA}, which affects how the player can manifest in the virtual world. The user can also see, hear, or speak through placed Alternate Avatars, allowing the user to be present in multiple locations at a moment's notice. For example, the user may place an Arcane Eye from the Eye Slot that he can use to see through even after leaving the area. 

The avatar equipment also serves as a metaphor for possession. The user may place equipment associated with different body parts on other characters to experience the world through their perspective. For example, the user may place the eye equipment at another user to request the VR equivalent of remote desktop to see what that user sees.

\textbf{Dev Set for Development or Debug}
In this Set, the user is provided equipment that is expected to aid with the development process. For example, the user may interact with the equipment at the eye slot to see the world as wireframe constructions, shaded, or in terms of UV Charts. Alternatively, with the Dev Set and context, the user may see code or data associated with each objects in the world and can then modify them. This functionality is likely to be app-specific. While any VR experience should have a Dev Set, it would not be universal as it is not intended for everyday users.

\textbf{App/Game-Specific Set}
Last but not least, are Sets that are specific to the application or the game the user is playing. For example, a user may be playing a fantasy VR games with swords and shields or the user may be using a VR drawing application and interact with the equipment in the eye slot to see different lighting conditions or different layers.
*/