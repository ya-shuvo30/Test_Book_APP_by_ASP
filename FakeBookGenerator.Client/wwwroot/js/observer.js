// This script sets up an IntersectionObserver to detect when a specific element
// becomes visible on the screen. When it does, it calls a .NET method.

let observer;

export function observe(element, dotNetHelper) {
    // Options for the observer (e.g., trigger when 10% of the element is visible)
    const options = {
        root: null,
        rootMargin: '0px',
        threshold: 0.1
    };

    // Create the observer
    observer = new IntersectionObserver(async (entries) => {
        // Check if the element is intersecting (visible)
        if (entries[0].isIntersecting) {
            // The element is visible, call the .NET method to load more books
            await dotNetHelper.invokeMethodAsync('LoadMoreBooks');
        }
    }, options);

    // Start observing the target element
    observer.observe(element);
}

export function unobserve(element) {
    if (observer && element) {
        observer.unobserve(element);
    }
}
