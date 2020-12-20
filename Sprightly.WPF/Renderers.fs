namespace Sprightly.WPF

open Xamarin.Forms.Platform.WPF

open Sprightly.Presentation.WPF

module Dummy_SprightlyListElementRenderer= 
    [<assembly: ExportRenderer(typeof<Sprightly.Presentation.Components.Common.SprightlyIconButton>, 
                               typeof<SprightlyIconButtonRenderer>)>]
    do ()

module Dummy_SprightlyButtonRenderer= 
    [<assembly: ExportRenderer(typeof<Sprightly.Presentation.Components.Common.SprightlyButton>, 
                               typeof<SprightlyButtonRenderer>)>]
    do ()

module Dummy_CollapsiblePaneHeaderRenderer= 
    [<assembly: ExportRenderer(typeof<Sprightly.Presentation.Components.Common.CollapsiblePaneHeader>, 
                               typeof<CollapsiblePaneHeaderRenderer>)>]
    do ()

module Dummy_RecentProjectButtonRenderer= 
    [<assembly: ExportRenderer(typeof<Sprightly.Presentation.Components.StartPage.RecentProjectButton>, 
                               typeof<RecentProjectButtonRenderer>)>]
    do ()
