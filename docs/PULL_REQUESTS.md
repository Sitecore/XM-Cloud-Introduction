# Pull Request Guidelines

Pull Requests are both a great way to maintain high quality code and an opportunity to learn from each other. Here are a few best practices to get you started:

## Submitting a Pull Request
Many times the pull request is the team's first opportunity to collaborate on an otherwise independent feature.  When creating a Pull Request, put yourself in the reviewer's seat and imagine that they have no context into the work that you've been doing.  The details that you share in your Pull Request help to build up that context facilitating crucial knowledge sharing within the team.

- It's much better to submit small pull requests often. If your changes involve more than 5-10 files, consider breaking it into smaller chunks. It's much easier for a reviewer to read through a four file change than it is to read through a massive refactor.
- Include a helpful PR description.This could include, but is not limited to:
  - A link to a issue, ticket or other request if there is one. You can link in most source control by just noting the ticket number---#542. In other situations, drop a URL. This gives the reviewer some context.
  - A brief explanation on why you are submitting the PR. This helps the reviewer, and also provides context if your change is referenced in the future.
  - A description of the changes, especially if there are a lot of file changes or multiple components being changed. For example, it is helpful to point out that file 1, 2, & 3 relate to this functionality update while files 4 & 5 relate to a different one
  - A screenshot or gif of the change. This does not prevent the reviewer from pulling down the code and verifying independently, but it does help with smaller styling changes or if the project is arduous to pull down and set up.
  - Steps to test. This could describe how to navigate to the changed component, what steps to perform with the component, etc. Example:
    > ### Steps To Test:
    > - Run Storybook
    > - Navigate to Pages > Homepage
    > - Confirm that this thing changed
    > - Confirm that when button is clicked, text updates

## Reviewing a Pull Request

- Who can review? Simple, everyone should review code. If you are new to a project, you'll get a feeling for the structure other projects. If you are experienced, you'll be able to offer more insights.
- Each project will need to decide who is able to officially approve a request. If you are principle reviewer, try to finish a review within 24 hours. When you approve a PR, leave a comment saying it looks good or a simple (thumbs up). Some systems have official approval buttons.
- Merging a pull request is the responsibility of the person who opened the PR. Why? There may be additional tasks such as deploying or updating a tag.
- When reviewing, open the code in a browser if possible. It can be hard to grasp a change until you see it live.
- Remember to be polite. As one guideline says "Accept that many programming decisions are opinions. Discuss tradeoffs, which you prefer, and reach a resolution quickly."
- We are all learning. As long as the code does not violate agreed standards, the author has a right to use whatever solution they want.
- If possible, don't just leave a comment about a potential issue, recommend a fix. Sketch the solution if possible.
- If you think there is a better solution, but are not sure, you can say that. Just note that it is a gut feeling and the code may be fine.
- A comment is the start of a conversation, you may need to clarify and expand on your suggestions.
- For more guidelines on review code, see the [thoughtbot guide](https://github.com/thoughtbot/guides/blob/main/code-review/README.md) and the [guide by github](https://docs.github.com/en/pull-requests/collaborating-with-pull-requests/getting-started/best-practices-for-pull-requests).
- Begin a comment with an emoji communicating the intent of the comment. These include:
  - :wrench: - An issue that needs to be fixed before the PR will be approved.
  - :information_desk_person: - An alternative solution or improvement that will improve the code but will not prevent the PR from being approved.
  - :question: - A question to help understand a portion of the code under review. Questions should be answered by the reviewee before PR approval.
  - :thought_balloon: - A comment or observation that will not prevent the PR from being approved.


## Receiving Feedback
- Assume everyone is acting in good faith. It's hard to communicate tone on the web. So assume that everyone is giving suggestions in a compassionate manner. It's almost certainly true.
- You have the right to say no. As long as you are not violating an agreed upon set of standards, it's your code. But if you disagree just note that you read the comment, but prefer to keep it how it is. It's helpful if you can give a justification---"I think a for loop communicates a little more clearly than a reduce function"---but it's not mandatory.
- If you like a suggestion, but do not have time, create an issue or leave a TODO statement in the code.